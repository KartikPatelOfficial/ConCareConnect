using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Api;

internal interface IPatientApi
{

    [Headers("Authorization: Bearer")]
    [Get("/patient")]
    Task<List<Patient>> GetPatients();

    [Headers("Authorization: Bearer")]
    [Post("/patient")]
    Task<Patient> AddPatient([Body] PatientRequest patient);

    [Headers("Authorization: Bearer")]
    [Get("/patient/{phone}")]
    Task<List<Patient>> GetPatientByPhone(string phone);

}
