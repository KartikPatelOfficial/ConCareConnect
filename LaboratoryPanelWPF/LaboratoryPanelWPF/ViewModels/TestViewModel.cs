using DiagnosticServicesModel.DataClass;
using LaboratoryPanelWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;

namespace LaboratoryPanelWPF.ViewModels
{
    internal class TestReportViewModel : FormViewModel
    {
        private readonly Test test;

        public TestReportViewModel(List<TestViewModel> testReport, Test test)
        {
            TestReports = testReport;
            this.test = test;

            TestCollectionView = CollectionViewSource.GetDefaultView(testReport);
            TestCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));

            TestCollectionView.Refresh();
        }

        public string Category => test.Name;

        public string Note => test.Categorynote;
        public long TestId => test.Testid;
        public long CategoryId => test.Id;

        public ICollectionView TestCollectionView { get; }
        public List<TestViewModel> TestReports { get; }
    }

    class TestViewModel : FormViewModel
    {
        private readonly Parameter testReport;
        private readonly string gender;

        public TestViewModel(Parameter testReport, string gender)
        {
            this.testReport = testReport;
            this.gender = gender;
        }

        public string TestName => testReport.TestName;
        public string Unit => testReport.Unit;
        public string GroupName => testReport.TestGroupPrintingName;

        public string NormalRange => testReport.TestFor == TestFor.All
            ? testReport.CommonNormalRange
            : gender == "Male"
                ? testReport.MaleNormalRange
                : testReport.FemaleNormalRange;

        private string result = string.Empty;

        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public ResultType ResultType
        {
            get
            {
                ResultType resultType = ResultType.NORMAL;

                if (result == null || result == "") return resultType;

                var parsed = double.TryParse(result, out var parsedResult);

                if (!parsed) return resultType;

                switch (testReport.TestFor)
                {
                    case TestFor.All:
                        {
                            if (parsedResult <= testReport.CommonLowerRange)
                            {
                                resultType = ResultType.LOW;
                            }
                            else if (parsedResult >= testReport.CommonUpperRange)
                            {
                                resultType = ResultType.HIGH;
                            }
                            break;
                        }
                    case TestFor.Gender:
                        {
                            if (gender == "Male")
                            {
                                if (parsedResult <= testReport.MaleLowerRange)
                                {
                                    resultType = ResultType.LOW;
                                }
                                else if (parsedResult >= testReport.MaleUpperRange)
                                {
                                    resultType = ResultType.HIGH;
                                }
                            }
                            else
                            {
                                if (parsedResult <= testReport.FemaleLowerRange)
                                {
                                    resultType = ResultType.LOW;
                                }
                                else if (parsedResult >= testReport.FemaleUpperRange)
                                {
                                    resultType = ResultType.HIGH;
                                }
                            }
                            break;
                        }
                }

                return resultType;
            }
        }

        public bool IsCritical
        {
            get
            {
                if (testReport.CommonLowerRange == 0 && testReport.MaleLowerRange == 0 && testReport.CommonUpperRange == 0 && testReport.MaleUpperRange == 0)
                {
                    return false;
                }
                var valid = double.TryParse(result, out var parsedResult) && testReport.TestFor switch
                {
                    TestFor.All => !(parsedResult <= testReport.CommonUpperRange) ||
                                      !(parsedResult >= testReport.CommonLowerRange),
                    TestFor.Gender => gender == "Male"
                        ? !(parsedResult <= testReport.MaleUpperRange) || !(parsedResult >= testReport.MaleLowerRange)
                        : !(parsedResult <= testReport.FemaleUpperRange) ||
                          !(parsedResult >= testReport.FemaleLowerRange),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return valid;
            }

        }

        public SolidColorBrush ForegroundBrush
        {
            get
            {
                return IsCritical
                    ? new SolidColorBrush(Colors.Red)
                    : new SolidColorBrush(Colors.Black);
            }

        }
    }
}
