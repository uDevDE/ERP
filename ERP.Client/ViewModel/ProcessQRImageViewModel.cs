using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace ERP.Client.ViewModel
{
    public class ProcessQRImageViewModel : INotifyPropertyChanged
    {
        private ImageSource _qrImageSource;

        public ImageSource QRImageSource
        {
            get { return _qrImageSource; }
            set
            {
                if (_qrImageSource != value)
                {
                    _qrImageSource = value;
                    RaisePropertyChanged("QRImageSource");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
