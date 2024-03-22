using CommunityToolkit.Mvvm.Input;
using DiagnosticServicesModel.Request;
using LaboratoryPanelWPF.Utils;
using System.ComponentModel.DataAnnotations;

namespace LaboratoryPanelWPF.ViewModels
{
    public class AddReferenceViewModel : FormViewModel
    {
        #region Properties
        private string _name = string.Empty;
        private string _qualification = string.Empty;
        private string _address = string.Empty;
        private string _phone = string.Empty;
        private string _hospital = string.Empty;
        private int _cutoff;

        [Required(ErrorMessage = "Name is required")]
        public string Name
        {
            get => _name;
            set
            {
                _ = ValidateProperty(value);
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Qualification
        {
            get => _qualification;
            set
            {
                _ = ValidateProperty(value);
                _qualification = value;
                OnPropertyChanged(nameof(Qualification));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _ = ValidateProperty(value);
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _ = ValidateProperty(value);
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Hospital
        {
            get => _hospital;
            set
            {
                _ = ValidateProperty(value);
                _hospital = value;
                OnPropertyChanged(nameof(Hospital));
            }
        }

        [Required(ErrorMessage = "Cut off is required")]
        public int CutOff
        {
            get => _cutoff;
            set
            {
                _ = ValidateProperty(value);
                _cutoff = value;
                OnPropertyChanged(nameof(CutOff));
            }
        }
        #endregion

        private readonly ICallback _callback;

        #region Commands
        public RelayCommand AddCommand { get; set; }
        #endregion

        public AddReferenceViewModel(ICallback callback)
        {
            AddCommand = new RelayCommand(Add);
            this._callback = callback;
        }

        private void Add()
        {
            ValidateProperty(Name);
            ValidateProperty(Qualification);
            ValidateProperty(Address);
            ValidateProperty(Phone);
            ValidateProperty(Hospital);
            ValidateProperty(CutOff);
            ValidateProperty(Address);

            if (HasErrors)
            {
                return;
            }

            var reference = new ReferenceRequest
            {
                Name = Name,
                Qualification = Qualification,
                Address = Address,
                Phone = Phone,
                Hospital = Hospital,
                Cutoff = CutOff
            };

            _callback.Callback(reference, null);
        }
    }
}
