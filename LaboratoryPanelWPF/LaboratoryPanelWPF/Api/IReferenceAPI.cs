using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Api;

public interface IReferenceApi
{
    [Headers("Authorization: Bearer")]
    [Get("/Reference")]
    Task<List<Reference>> GetAllReferences();

    [Headers("Authorization: Bearer")]
    [Post("/Reference")]
    Task<Reference> AddReference([Body] ReferenceRequest reference);

    [Headers("Authorization: Bearer")]
    [Put("/Reference")]
    Task<Reference> UpdateReference([Body] ReferenceRequest reference);

    [Headers("Authorization: Bearer")]
    [Delete("/Reference/{id}")]
    Task DeleteReference(int id);
}