using System;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public interface IAppointmentService
    {

        public Task<List<Appointment>> GetAppointments();

        public Task<Appointment> AddAppointment(AppointmentRequest appointment);

        public Task AddTests(Guid id,List<TestResultRequest> testResultRequest);
        
        public Task DeleteAppointment(Guid id);

    }
}
