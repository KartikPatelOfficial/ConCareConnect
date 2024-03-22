using CommunityToolkit.Mvvm.Input;
using DiagnosticServicesModel.DataClass;
using LaboratoryPanelWPF.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using LaboratoryPanelWPF.Service;
using Wpf.Ui;

namespace LaboratoryPanelWPF.ViewModels
{
    public class AppointmentDetailViewModel : ViewModelBase
    {
        private readonly TestReportGenerator testReportGenerator;
        private readonly INavigationService navigationService;
        private readonly IAppointmentService appointmentService;
        
        public ICollectionView CategoryCollectionView { get; }

        public ICommand PrintReportCommand { get; }
        
        public ICommand OnClickDeleteCommand { get; }


        public AppointmentDetailViewModel(
            Appointment appointment,
            IAppointmentService appointmentService,
            INavigationService navigationService
            )
        {
            AppointmentResponse = appointment;
            this.appointmentService = appointmentService;
            this.navigationService = navigationService;
            CategoryCollectionView = CollectionViewSource.GetDefaultView(appointment.Tests);
            CategoryCollectionView.Refresh();

            PrintReportCommand = new RelayCommand(PrintReport);
            OnClickDeleteCommand = new RelayCommand( OnClickDelete);
            testReportGenerator = new TestReportGenerator();
        }

        public string Initial => AppointmentResponse.Patient.Name.Split().First();
        public string Name => string.Join(" ", AppointmentResponse.Patient.Name.Split().Skip(1));

        public string PatientDetail
        {
            get
            {
                var age = DateTime.Today.Year - AppointmentResponse.Patient.Dob!.Value.Year;
                return $"{age} Year {AppointmentResponse.Patient.Gender}";
            }
        }

        public string DoctorName => AppointmentResponse.Doctor.Name;
        public string SecondDoctorName => AppointmentResponse.SecondDoctor?.Name ?? "";

        public string? MedicalHistory => AppointmentResponse.Patient.Medicalhistory;
        public string? Remarks => AppointmentResponse.Patient.Remarks;

        public Appointment AppointmentResponse { get; }

        public string TestCount => $"{AppointmentResponse.Tests.Count} Test" + (AppointmentResponse.Tests.Count < 1 ? "" : "s");

        public decimal Total
        {
            get
            {
                if (AppointmentResponse.Total != 0)
                {
                    return AppointmentResponse.Total;
                }

                return AppointmentResponse.Tests.Sum(test => test.Cost) ?? 0;
            }
        }

        private void PrintReport()
        {

            var generatedPdfPath = testReportGenerator.GenerateReport(AppointmentResponse.Tests.ToList(),AppointmentResponse);
            
            // open the generated report
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(generatedPdfPath)
            {
                UseShellExecute = true
            };
            p.Start();
            
        }
        
        private  void OnClickDelete()
        {
            appointmentService.DeleteAppointment(AppointmentResponse.Id);
            navigationService.GoBack();

        }
        
    }
}
