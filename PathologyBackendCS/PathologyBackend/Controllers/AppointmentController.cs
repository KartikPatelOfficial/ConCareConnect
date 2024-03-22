using System.Security.Claims;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathologyBackend.Models;


namespace PathologyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly DiagnosticServiceContext _context;

        public AppointmentController(DiagnosticServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentRequest appointmentRequest)
        {
            if (appointmentRequest == null)
            {
                return BadRequest();
            }

            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest("Laboratory not found");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == appointmentRequest.Patientid);

            if (patient == null)
            {
                return BadRequest("Patient not found");
            }

            var appointment = new Appointment
            {
                Laboratoryid = uid,
                Patientid = patient.Id,
                Status = "CREATED",
                Doctorid = appointmentRequest.Doctorid,
                SecondDoctorid = appointmentRequest.SecondDoctorid,
                Createdat = DateTime.Now,
                Reportedat = DateTime.Now,
                Sampletype = appointmentRequest.Sampletype,
                Patienttype = appointmentRequest.Patienttype,
                Total = appointmentRequest.Total,
                Labcharges = appointmentRequest.Labcharges,
                Expenses = appointmentRequest.Expenses,
                Doctormargin = appointmentRequest.Doctormargin,
                Paymentstatus = appointmentRequest.Paymentstatus
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var tests = appointmentRequest.Tests.Select(t => new Test
                {
                Appointmentid = appointment.Id,
                Laboratoryid = uid,
                Testid = t.TestId,
                Name = t.TestName,
                Categorynote = t.TestDescription,
                Cost = t.TestPrice,
                Expenses = decimal.Parse(t.Expenses.ToString()),
                Status = "NEW",
                Createdat = DateTime.Now
            }
            );

            foreach (var test in tests)
            {
                _context.Tests.Add(test);
            }

            await _context.SaveChangesAsync();

            return Ok(appointment);
        }


        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest("Laboratory not found");
            }

            var appointments = await _context.Appointments.Where(a => a.Laboratoryid == uid).ToListAsync();
            appointments.ForEach(appointments => appointments.Tests = _context.Tests.Where(t => t.Appointmentid == appointments.Id).ToList());
            appointments.ForEach(appointments => appointments.Doctor = _context.References.FirstOrDefault(d => d.Id == appointments.Doctorid));
            appointments.ForEach(appointments => appointments.SecondDoctor = _context.References.FirstOrDefault(d => d.Id == appointments.SecondDoctorid));
            appointments.ForEach(appointments => appointments.Patient = _context.Patients.FirstOrDefault(p => p.Id == appointments.Patientid));

            return Ok(appointments);
        }

        [HttpPut("{appointmentId}/result")]
        public async Task<IActionResult> AddResult(Guid appointmentId, List<TestResultRequest> testResultRequests)
        {
            if (testResultRequests == null)
            {
                return BadRequest();
            }

            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest("Laboratory not found");
            }

            foreach (var testResultRequest in testResultRequests)
            {
                var test = await _context.Tests.FirstOrDefaultAsync(t => t.Id == testResultRequest.Testid);

                if (test == null)
                {
                    return BadRequest("Test not found");
                }
                var parameters = testResultRequest.Parameters.Select(p => p.ToString()).ToArray();
                test.Result = parameters;
                test.Status = "FINISHED";

                _context.Tests.Update(test);
            }

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                return BadRequest("Appointment not found");
            }

            appointment.Status = "SUBMITTED";
            appointment.Reportedat = DateTime.Now;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Appointment updated {appointment}");

            return Ok(appointment);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return BadRequest("Appointment not found");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);
        }
    }
}