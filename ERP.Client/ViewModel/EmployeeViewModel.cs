using ERP.Client.Model;
using System.ComponentModel;

namespace ERP.Client.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private EmployeeModel _employee;
        public EmployeeModel Employee
        {
            get { return _employee; }
            set
            {
                if (_employee != value)
                {
                    _employee = value;
                    RaisePropertyChanged("Employee");
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string Name
        {
            get
            {
                if (_employee != null)
                    return string.Format("{0:s}, {1:s}", _employee.Lastname, _employee.Firstname);

                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
