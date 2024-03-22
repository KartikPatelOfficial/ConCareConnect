using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Utils;
using Refit;

namespace LaboratoryPanelWPF.ViewModels
{
    public class AddResultViewModel : ViewModelBase
    {
        public ICollectionView CategoryCollectionView { get; }

        private readonly List<TestReportViewModel> _tests = new();
        private readonly Appointment _appointment;

        private readonly IAppointmentService _appointmentService;

        public ICommand SaveAndPrintCommand { get; }

        public AddResultViewModel(Appointment appointment, IDBService dBService, IAppointmentService appointmentService)
        {
            this._appointment = appointment;
            _appointmentService = appointmentService;
            SaveAndPrintCommand = new RelayCommand(SaveAndPrint);

            foreach (var test in appointment.Tests)
            {
                var parameters = dBService.GetParameters(test.Testid);
                var categoryTests = parameters.Select(testReport => new TestViewModel(testReport, appointment.Patient.Gender)).ToList();

                _tests.Add(new TestReportViewModel(categoryTests, test));
            }

            CategoryCollectionView = CollectionViewSource.GetDefaultView(_tests);
        }


        private async void SaveAndPrint()
        {
            var testResultRequests = _tests.Select(test => new TestResultRequest()
            {
                Testid = test.CategoryId,
                Parameters = test.TestReports.Select(parameter => new ParameterRequest()
                {
                    Name = parameter.TestName,
                    Value = parameter.Result,
                    Unit = parameter.Unit,
                    Normalrange = parameter.NormalRange,
                    IsCritical = parameter.IsCritical,
                    GroupName = parameter.GroupName
                }).ToList()
            }).ToList();

            try
            {
                await _appointmentService.AddTests(_appointment.Id, testResultRequests);
                var testReportGenrator = new TestReportGenerator();

                var path = testReportGenrator.GenerateReport(_appointment.Tests.ToList(), _appointment);

                // open the report

            }
            catch (ApiException e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show(e.Message);

            }
        }

    }
}
