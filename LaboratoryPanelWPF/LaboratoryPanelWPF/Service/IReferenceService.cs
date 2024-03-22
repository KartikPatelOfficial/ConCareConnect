using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public interface IReferenceService
    {
        Task<List<Reference>> GetAllReferencesAsync();

        Task<Reference> AddReferenceAsync(ReferenceRequest reference);

        Task<Reference> UpdateReferenceAsync(ReferenceRequest reference);

        Task DeleteReferenceAsync(int id);
    }
}
