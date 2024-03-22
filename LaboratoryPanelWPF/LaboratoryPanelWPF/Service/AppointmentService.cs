using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Api;
using LaboratoryPanelWPF.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentApi _appointmentAPI = RetrofitClient.Instance.Create<IAppointmentApi>();

        public async Task<Appointment> AddAppointment(AppointmentRequest appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }
            try
            {
                return await _appointmentAPI.AddAppointment(appointment);
            }
            catch (Exception e)
            {
                
                throw;
            }
        }

        public Task AddTests(Guid appointmentId, List<TestResultRequest> testResultRequest)
        {
            if (testResultRequest == null)
            {
                throw new ArgumentNullException(nameof(testResultRequest));
            }
            try
            {
                return _appointmentAPI.AddResult(appointmentId, testResultRequest);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Appointment>> GetAppointments()
        {
            return _appointmentAPI.GetAppointments();
        }
        
        public Task DeleteAppointment(Guid id)
        {
            return _appointmentAPI.DeleteAppointment(id);
        }

    }
}
