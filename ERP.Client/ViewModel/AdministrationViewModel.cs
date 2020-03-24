using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.ViewModel
{
    public class AdministrationViewModel : INotifyPropertyChanged
    {
        private bool _isAdministrator;
        public bool IsAdministrator
        {
            get { return _isAdministrator; }
            set
            {
                if (_isAdministrator != value)
                {
                    _isAdministrator = value;
                    RaisePropertyChanged();
                }
            }
        }

        //public AdministrationViewModel() => IsAdministrator = true;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
