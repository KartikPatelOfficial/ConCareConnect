using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public interface IPatientService
    {

        public Task<List<Patient>> GetPatients();

        public Task<Patient> AddPatient(PatientRequest patient);

        public Task<List<Patient>> GetPatientByPhone(string phone);

    }
}
