using CommunityToolkit.Mvvm.Input;
using DiagnosticServicesModel.DataClass;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Views.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Wpf.Ui;

namespace LaboratoryPanelWPF.ViewModels
{
    public partial class AllAppointmentsPageViewModel : FormViewModel
    {
        private readonly List<Appointment> _appointments;

        private readonly IAppointmentService _appointmentService;
        private readonly INavigationService _navigationService;
        private readonly IDBService _dBService;

        public ICollectionView Appointments { get; }

        public ICommand RunAddAppointmentCommand => new RelayCommand(AddAppointment);

        public ICommand RunViewAppointmentCommand => new Utils.RelayCommand<Appointment>(ViewAppointment);

        public AllAppointmentsPageViewModel(IAppointmentService appointmentService, INavigationService navigationService, IDBService dBService)
        {
            _appointmentService = appointmentService;
            _dBService = dBService;
            _navigationService = navigationService;

            _appointments = new List<Appointment>();

            Appointments = CollectionViewSource.GetDefaultView(_appointments);


            _ = LoadAppointments();
        }


        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                Appointments.Filter = item =>
                {
                    if (string.IsNullOrEmpty(_searchString))
                        return true;
                    else
                    {
                        var appointment = item as Appointment;
                        return
                            appointment.Patient.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                            appointment.Doctor.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                            appointment.Createdat.Value.ToShortTimeString().Contains(_searchString, StringComparison.OrdinalIgnoreCase);
                    }
                };
                Appointments.Refresh();
            }
        }

        private void ViewAppointment(Appointment appointment)
        {

            switch (appointment.Status)
            {
                case "CREATED":
                    _navigationService.NavigateWithHierarchy(typeof(AddResultWindow), new AddResultViewModel(appointment, _dBService, _appointmentService));
                    break;
                case "SUBMITTED":
                    _navigationService.NavigateWithHierarchy(typeof(AppointmentDetailWindow), new AppointmentDetailViewModel(appointment, _appointmentService, _navigationService));
                    break;
            }

        }

        [RelayCommand]
        public async Task RunRefreshAppointments()
        {
            await LoadAppointments();
        }


        private void AddAppointment()
        {
            _navigationService.NavigateWithHierarchy(typeof(PatientPage));
        }

        private async Task LoadAppointments()
        {
            _appointments.Clear();

            var appointments = await _appointmentService.GetAppointments();

            foreach (var appointment in appointments)
            {
                _appointments.Add(appointment);
            }
            Appointments.Refresh();
        }


    }
}
