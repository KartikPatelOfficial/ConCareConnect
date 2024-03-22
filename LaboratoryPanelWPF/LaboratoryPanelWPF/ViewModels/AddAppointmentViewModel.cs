using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Views.Dialog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Refit;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Utils;
using Wpf.Ui;
using CommunityToolkit.Mvvm.Input;

namespace LaboratoryPanelWPF.ViewModels
{
    public partial class AddAppointmentViewModel : FormViewModel, ICallback
    {
        private readonly IDBService _dbService;
        private readonly IReferenceService _referenceService;
        private readonly IAppointmentService _appointmentService;
        private readonly INavigationService _navigationService;


        public ICommand DeleteTestCommand => new Wpf.Ui.Input.RelayCommand<object>(DeleteTest);
        public ICommand SaveTestCommand => new Wpf.Ui.Input.RelayCommand<object>(SaveTest);
        public ICommand AddReferenceCommand => new Wpf.Ui.Input.RelayCommand<object>(AddReference);


        public static Patient Patient { get; set; }


        public AddAppointmentViewModel(IDBService dbService, IReferenceService referenceService,
            IAppointmentService appointmentService, INavigationService navigationService)
        {
            _dbService = dbService;
            _referenceService = referenceService;
            _appointmentService = appointmentService;
            _navigationService = navigationService;

            AllTestsCollectionView = new ObservableCollection<Category>();
            SelectedTestsCollectionView = new ObservableCollection<Category>();

            LoadReferences();
            Load();
        }

        private async void Load()
        {
            var tests = _dbService.GetCategories();
            foreach (var test in tests)
            {
                AllTestsCollectionView.Add(test);
            }

            try
            {
                LoadReferences();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ObservableCollection<Category> AllTestsCollectionView { get; }
        public ObservableCollection<Category> SelectedTestsCollectionView { get; }

        public ObservableCollection<Reference> Doctors { get; } = new();

        #region Properties

        private int discountPercent;

        public int DiscountPercent
        {
            get => discountPercent;
            set
            {
                discountPercent = value;
                var d = value / 100d * SubTotal;
                discount = Convert.ToInt32(d);
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(GrandTotal));
            }
        }

        private int discount;

        public int Discount
        {
            get => discount;
            set
            {
                discount = value;
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(GrandTotal));
            }
        }

        private int paid;

        public int Paid
        {
            get => paid;
            set
            {
                paid = value;
                OnPropertyChanged(nameof(Paid));
            }
        }

        public int SubTotal => Convert.ToInt32(SelectedTestsCollectionView.Sum(model => model.Cost));

        public int GrandTotal => SubTotal - discount;

        private void DeleteTest(object? item)
        {
            SelectedTestsCollectionView.Remove(item as Category);
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(GrandTotal));
        }

        private Reference selectedDoctor;
        private Reference selectedSecondDoctor;

        [Required(ErrorMessage = "required.")]
        public Reference Doctor
        {
            get => selectedDoctor;
            set
            {
                selectedDoctor = value;
                OnPropertyChanged(nameof(Doctor));
            }
        }

        public Reference SecondDoctor
        {
            get => selectedSecondDoctor;
            set
            {
                selectedSecondDoctor = value;
                OnPropertyChanged(nameof(SecondDoctor));
            }
        }

        private string sampleType = string.Empty;

        public string SampleType
        {
            get => sampleType;

            set
            {
                sampleType = value;
                OnPropertyChanged(nameof(SampleType));
            }
        }

        private string patientType = string.Empty;

        public string PatientType
        {
            get => patientType;

            set
            {
                patientType = value;
                OnPropertyChanged(nameof(PatientType));
            }
        }

        #endregion

        public void AddTest(Category test)
        {
            if (SelectedTestsCollectionView.Any(model => model.Id == test.Id)) return;

            SelectedTestsCollectionView.Add(test);
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(GrandTotal));
        }

        public void DeleteTest(object item, System.Action _)
        {
            SelectedTestsCollectionView.Remove(item as Category ?? throw new InvalidOperationException());
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(GrandTotal));
        }

        private async void LoadReferences()
        {
            try
            {
                var references = await _referenceService.GetAllReferencesAsync();
                Doctors.Clear();
                foreach (var reference in references)
                {
                    Doctors.Add(reference);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddReference(object? _)
        {
            var dialog = new AddReferenceWindow(this);
            dialog.ShowDialog();
        }

        [RelayCommand]
        public void GenerateInvoice(object? _)
        {
            var tests = SelectedTestsCollectionView.Select(model => new TestRequest()
            {
                TestId = model.Id,
                TestPrice = decimal.Parse(model.Cost.ToString(CultureInfo.InvariantCulture)),
                TestName = model.Name,
                TestDescription = "-",
                Expenses = 0
            }).ToList();

            var appointmentRequest = new AppointmentRequest
            {
                Patientid = Patient.Id,
                Doctorid = Doctor.Id,
                SecondDoctorid = SecondDoctor?.Id,
                Patienttype = PatientType,
                Sampletype = SampleType,
                Total = SubTotal,
                Labcharges = 0,
                Expenses = 0,
                Doctormargin = Doctor.Cutoff,
                Paymentstatus = "UNPAID",
                Tests = tests
            };

            var invoice = new Invoice(appointmentRequest, GrandTotal, Discount, Paid);

            var path = InvoiceGenerator.GenerateInvoice(invoice, Patient);

            MessageBox.Show($"Invoice generated at {path}");
        }


        private async void SaveTest(object? _)
        {
            var tests = SelectedTestsCollectionView.Select(model => new TestRequest()
            {
                TestId = model.Id,
                TestPrice = decimal.Parse(model.Cost.ToString(CultureInfo.InvariantCulture)),
                TestName = model.Name,
                TestDescription = "-",
                Expenses = 0
            }).ToList();

            var appointmentRequest = new AppointmentRequest
            {
                Patientid = Patient.Id,
                Doctorid = Doctor.Id,
                SecondDoctorid = SecondDoctor?.Id,
                Patienttype = PatientType,
                Sampletype = SampleType,
                Total = SubTotal,
                Labcharges = 0,
                Expenses = 0,
                Doctormargin = Doctor.Cutoff,
                Paymentstatus = "UNPAID",
                Tests = tests
            };
            try
            {
                var appointment = await _appointmentService.AddAppointment(appointmentRequest);

                _navigationService.GoBack();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        [RelayCommand]
        public void GoBack()
        {
            _navigationService.GoBack();
        }

        public async void Callback(object sender, EventArgs e)
        {
            if (sender is not ReferenceRequest referenceRequest) return;
            try
            {
                var reference = await _referenceService.AddReferenceAsync(referenceRequest);
                Doctors.Add(reference);
            }
            catch (ApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}