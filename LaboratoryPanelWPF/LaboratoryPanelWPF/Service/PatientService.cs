using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Api;
using LaboratoryPanelWPF.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    internal class PatientService : IPatientService
    {
        private readonly IPatientApi _patientAPI = RetrofitClient.Instance.Create<IPatientApi>();


        public async Task<Patient> AddPatient(PatientRequest patientRequest)
        {
            if (patientRequest == null)
            {
                throw new ArgumentNullException(nameof(patientRequest));
            }

            Patient? patient;
            try
            {
                patient = await _patientAPI.AddPatient(patientRequest);
            }
            catch (Exception)
            {
                throw;
            }


            return patient;
        }

        public async Task<List<Patient>> GetPatientByPhone(string phone)
        {
            return await _patientAPI.GetPatientByPhone(phone);
        }

        public Task<List<Patient>> GetPatients()
        {
            return _patientAPI.GetPatients();
        }
    }
}
