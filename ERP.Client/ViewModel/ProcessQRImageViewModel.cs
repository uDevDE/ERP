using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ERP.Client.ViewModel
{
    public class ProcessQRImageViewModel : INotifyPropertyChanged
    {
        private BitmapImage _qrImageSource;

        public BitmapImage QRImageSource
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
