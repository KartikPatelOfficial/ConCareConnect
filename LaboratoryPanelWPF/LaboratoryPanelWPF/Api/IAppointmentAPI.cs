using System;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Api
{
    public interface IAppointmentApi
    {

        [Headers("Authorization: Bearer")]
        [Get("/appointment")]
        Task<List<Appointment>> GetAppointments();

        [Headers("Authorization: Bearer")]
        [Post("/appointment")]
        Task<Appointment> AddAppointment([Body] AppointmentRequest appointment);

        [Headers("Authorization: Bearer")]
        [Put("/appointment/{id}/result")]
        Task AddResult(Guid id, [Body] List<TestResultRequest> testResultRequests);

        [Headers("Authorization: Bearer")]
        [Delete("/appointment/{id}")]
        Task DeleteAppointment(Guid id);
    }
}
