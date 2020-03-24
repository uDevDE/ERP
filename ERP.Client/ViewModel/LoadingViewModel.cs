using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.ViewModel
{
    public class LoadingViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _text;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void Enable(string text)
        {
            IsBusy = true;
            Text = text;
        }

        public void Disable()
        {
            IsBusy = false;
            Text = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
