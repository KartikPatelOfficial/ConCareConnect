using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Api;
using LaboratoryPanelWPF.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public class ReferenceService : IReferenceService
    {
        private readonly IReferenceApi _referenceApi = RetrofitClient.Instance.Create<IReferenceApi>();


        public async Task<Reference> AddReferenceAsync(ReferenceRequest reference)
        {
            return await _referenceApi.AddReference(reference);
        }


        public async Task DeleteReferenceAsync(int id)
        {
            await _referenceApi.DeleteReference(id);
        }

        public async Task<List<Reference>> GetAllReferencesAsync()
        {
            return await _referenceApi.GetAllReferences();
        }

        public async Task<Reference> UpdateReferenceAsync(ReferenceRequest reference)
        {
            return await _referenceApi.UpdateReference(reference);
        }
    }
}
