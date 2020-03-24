using ERP.Contracts.Domain.Core;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace ERP.Client.Model
{
    public class EmployeeModel : IEmployee, INotifyPropertyChanged
    {
        private int _employeeId;
        private int _number;
        private string _firstname;
        private string _lastname;
        private string _description;
        private string _alias;
        private string _password;
        private long _permissions;
        private bool _isAdministrator;
        private DeviceModel _device;
        private string _color;

        public EmployeeModel() => _device = new DeviceModel();
        public EmployeeModel(EmployeeModel employee)
        {
            _employeeId = employee.EmployeeId;
            _number = employee.Number;
            _firstname = employee.Firstname;
            _lastname = employee.Lastname;
            _description = employee.Description;
            _alias = employee.Alias;
            _password = employee.Password;
            _permissions = employee.Permissions;
            _isAdministrator = employee.IsAdministrator;
            _device = employee.Device;
            _color = employee.Color;
        }

        public int EmployeeId
        {
            get { return _employeeId; }
            set
            {
                if (_employeeId != value)
                {
                    _employeeId = value;
                    RaisePropertyChanged("EmployeeId");
                }
            }
        }
        public int Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    RaisePropertyChanged("Number");
                }
            }
        }
        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    RaisePropertyChanged("Firstname");
                }
            }
        }
        public string Lastname
        {
            get { return _lastname; }
            set
            {
                if (_lastname != value)
                {
                    _lastname = value;
                    RaisePropertyChanged("Lastname");
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
        public string Alias
        {
            get { return _alias; }
            set
            {
                if (_alias != value)
                {
                    _alias = value;
                    RaisePropertyChanged("Alias");
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        public long Permissions
        {
            get { return _permissions; }
            set
            {
                if (_permissions != value)
                {
                    _permissions = value;
                    RaisePropertyChanged("Permissions");
                }
            }
        }
        public bool IsAdministrator
        {
            get { return _isAdministrator; }
            set
            {
                if (_isAdministrator != value)
                {
                    _isAdministrator = value;
                    RaisePropertyChanged("IsAdministrator");
                }
            }
        }
        public DeviceModel Device
        {
            get { return _device; }
            set
            {
                if (_device != value)
                {
                    _device = value;
                    RaisePropertyChanged("Device");
                }
            }
        }

        public string Fullname
        {
            get { return string.Format("{0:s} {1:s}", _firstname, _lastname); }
        }

        public System.Guid? DeviceId { get; set; }

        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    RaisePropertyChanged("DeviceId");
                    RaisePropertyChanged("Foreground");
                }
            }
        }

        public Brush Foreground
        {
            get
            {
                var uiSettings = new Windows.UI.ViewManagement.UISettings();
                var defaultColor = new SolidColorBrush(uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.AccentDark1));
                try
                {
                    if (!string.IsNullOrEmpty(_color))
                        return GetColorFromHex(_color);

                    return defaultColor;
                }
                catch
                {
                    return defaultColor;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static bool operator ==(EmployeeModel src, EmployeeModel dest)
        {
            if (ReferenceEquals(src, dest))
                return true;

            if ((object)src == null || (object)dest == null)
                return false;

            return  src.Alias == dest.Alias && 
                    src.Color == dest.Color && 
                    src.Description == dest.Description && 
                    src.Device == dest.Device && 
                    src.DeviceId == dest.DeviceId && 
                    src.EmployeeId == dest.EmployeeId && 
                    src.Firstname == dest.Firstname &&
                    src.Foreground == dest.Foreground && 
                    src.Fullname == dest.Fullname && 
                    src.IsAdministrator == dest.IsAdministrator && 
                    src.Lastname == dest.Lastname && 
                    src.Number == dest.Number && 
                    src.Password == dest.Password &&
                    src.Permissions == dest.Permissions;
        }

        public static bool operator !=(EmployeeModel src, EmployeeModel dest) => !(src == dest);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private static SolidColorBrush GetColorFromHex(string hexString)
        {
            Color color = (Color)XamlBindingHelper.ConvertValue(typeof(Color), hexString);
            return new SolidColorBrush(color);
        }

    }
}
