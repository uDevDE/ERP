using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace ERP.Client.Model
{
    public class ProcessQRImageModel : INotifyPropertyChanged
    {
        private string _processName;
        private ImageSource _source;

        public string ProcessName
        {
            get { return _processName; }
            set
            {
                if (_processName != value)
                {
                    _processName = value;
                    RaiseProperyChanged("ProcessName");
                }
            }
        }

        public ImageSource Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaiseProperyChanged("Source");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
