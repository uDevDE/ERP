using ERP.Client.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ERP.Client.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private EmployeeModel _selectedEmployee;

        public ObservableCollection<EmployeeModel> Employees { get; private set; }

        public EmployeeViewModel() => Employees = new ObservableCollection<EmployeeModel>();

        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    RaisePropertyChanged("SelectedEmployee");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
