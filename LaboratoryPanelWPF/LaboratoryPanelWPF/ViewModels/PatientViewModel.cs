using CommunityToolkit.Mvvm.Input;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;

namespace LaboratoryPanelWPF.ViewModels
{
    public class PatientViewModel : FormViewModel
    {

        private readonly IPatientService _patientService;
        private readonly INavigationService _navigationService;

        private List<Patient> _patients;

        public ICommand RunSaveCommand { get; private set; }
        public ICommand RunSearchCommand { get; private set; }

        public ObservableCollection<Patient> PatientsCollection { get; private set; }

        public PatientViewModel(IPatientService patientService, INavigationService navigationService)
        {
            _patients = new List<Patient>();

            _patientService = patientService;
            _navigationService = navigationService;

            RunSaveCommand = new RelayCommand(SaveNewPatient);
            RunSearchCommand = new RelayCommand(SearchPatient);

            PatientsCollection = new ObservableCollection<Patient>();
        }


        #region Properties
        private string initial;
        private string firstName;
        private string lastName;
        private string gender;
        private string email;
        private string phone;
        private string street;
        private string city;
        private string pincode;
        private string medicalHistory;
        private DateTime dob;
        private string remarks;

        [Required(ErrorMessage = "required.")]
        public string Initial
        {
            get => initial;

            set
            {
                ValidateProperty(value);
                initial = value;
                OnPropertyChanged(nameof(Initial));
            }
        }

        public List<string> Initials => new List<string> { "Mr.", "Mrs.", "Miss." };
        public List<string> Genders => new List<string> { "Male", "Female", "Others" };

        [Required(ErrorMessage = "required.")]
        public string Gender
        {
            get => gender;

            set
            {
                ValidateProperty(value);
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        [Required(ErrorMessage = "required.")]
        public DateTime DOB
        {
            get => dob;

            set
            {
                ValidateProperty(value);
                dob = value;
                OnPropertyChanged(nameof(DOB));
            }
        }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName
        {
            get => firstName;
            set
            {
                ValidateProperty(value);
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName
        {
            get => lastName;
            set
            {
                ValidateProperty(value);
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        [EmailAddress]
        public string Email
        {
            get => email;
            set
            {
                OnPropertyChanged(nameof(Email));
                email = value;
            }
        }

        [Required(ErrorMessage = "Phone number is required"), Phone]
        public string Phone
        {
            get => phone;
            set
            {
                OnPropertyChanged(nameof(Phone));
                phone = value;
            }
        }

        [Required(ErrorMessage = "Street name is required.")]
        public string Street
        {
            get => street;
            set
            {
                OnPropertyChanged(nameof(Street));
                street = value;
            }
        }

        [Required(ErrorMessage = "City is required.")]
        public string City
        {
            get => city;
            set
            {
                OnPropertyChanged(nameof(City));
                city = value;
            }
        }

        [Required(ErrorMessage = "Pincode is required.")]
        [RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Please enter valid pincode.")]
        public string Pincode
        {
            get => pincode;
            set
            {
                OnPropertyChanged(nameof(Pincode));
                pincode = value;
            }
        }

        public string Remarks
        {
            get => remarks;
            set
            {
                OnPropertyChanged(nameof(Remarks));
                remarks = value;
            }
        }

        public string MedicalHistory
        {
            get => medicalHistory;
            set
            {
                OnPropertyChanged(nameof(medicalHistory));
                medicalHistory = value;
            }
        }
        #endregion

        private async void SaveNewPatient()
        {
            var patientRequest = ValidateAndGetPatient();
            if (patientRequest == null) return;
            try
            {
                var patient = await _patientService.AddPatient(patientRequest);

                AddAppointmentViewModel.Patient = patient;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _navigationService.GetNavigationControl().NavigateWithHierarchy(typeof(AddAppointmentPage));
        }

        private async void SearchPatient()
        {
            ValidateProperty(Phone, propertyName: nameof(Phone));
            if (HasErrors) return;

            try
            {
                _patients = await _patientService.GetPatientByPhone(Phone);
                PatientsCollection.Clear();
                foreach (var patient in _patients)
                {
                    PatientsCollection.Add(patient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private PatientRequest? ValidateAndGetPatient()
        {
            ValidateProperty(Initial, propertyName: nameof(Initial));
            ValidateProperty(FirstName, propertyName: nameof(FirstName));
            ValidateProperty(LastName, propertyName: nameof(LastName));
            ValidateProperty(Email, propertyName: nameof(Email));
            ValidateProperty(Phone, propertyName: nameof(Phone));
            ValidateProperty(Street, propertyName: nameof(Street));
            ValidateProperty(City, propertyName: nameof(City));
            ValidateProperty(Pincode, propertyName: nameof(Pincode));
            ValidateProperty(Gender, propertyName: nameof(Gender));
            ValidateProperty(DOB, propertyName: nameof(DOB));
            if (HasErrors) return null;

            var name = $"{Initial} {FirstName} {LastName}";
            var address = $"{Street}\n{City}, {Pincode}";
            return new PatientRequest(
                name: name,
                email: Email,
                phone: Phone,
                address: address,
                remarks: Remarks,
                medicalhistory: MedicalHistory,
                gender: Gender,
                dob: DOB);
        }

    }
}
