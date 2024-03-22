using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PathologyBackend.Models;
using System.Security.Claims;

namespace PathologyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly DiagnosticServiceContext _context;

        public PatientController(DiagnosticServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientRequest request)
        {

            if (request == null)
            {
                return BadRequest();
            }

            if (PatientExists(request.Email))
            {
                return BadRequest("Patient already exists");
            }


            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest("Laboratory not found");
            }

            var patient = new Patient()
            {
                Laboratoryid = uid,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Remarks = request.Remarks,
                Medicalhistory = request.Medicalhistory,
                Createdat = DateTime.Now,
                Gender = request.Gender,
                Dob = request.Dob
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return Ok(patient);
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest("Laboratory not found");
            }

            var patients = await _context.Patients.Where(p => p.Laboratoryid == uid).ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{phone}")]
        public async Task<IActionResult> GetPatient(string phone)
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest("Laboratory not found");
            }

            var patients = await _context.Patients.Where(p => p.Laboratoryid == uid && p.Phone == phone).ToListAsync();

            if (patients.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(patients);
        }

        private bool PatientExists(string? email)
        {
            if (email == null) return false;
            return _context.Patients.Any(e => e.Email == email);
        }

    }
}
