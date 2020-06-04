using System.ComponentModel;

namespace ERP.Client.Model
{
    public class ProjectNumberModel : INotifyPropertyChanged
    {
        private string _number;
        private string _name;
        private string _displayText;

        public string Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    _displayText = string.Format("{0:s} - {1:s}", _number, _name);
                    RaisePropertyChanged("Number");
                    RaisePropertyChanged("DisplayText");
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    _displayText = string.Format("{0:s} - {1:s}", _number, _name);
                    RaisePropertyChanged("Name");
                    RaisePropertyChanged("DisplayText");
                }
            }
        }

        public string DisplayText
        {
            get { return _displayText; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
