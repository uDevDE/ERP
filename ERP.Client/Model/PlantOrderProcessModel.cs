using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class PlantOrderProcessModel : INotifyPropertyChanged
    {
        private int _processId;
        private string _process;
        private int _processNumber;
        private System.Guid _processGuid;
        private int _groupId;

        public int ProcessId
        {
            get { return _processId; }
            set
            {
                if (_processId != value)
                {
                    _processId = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Process
        {
            get { return _process; }
            set
            {
                if (_process != value)
                {
                    _process = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ProcessNumber
        {
            get { return _processNumber; }
            set
            {
                if (_processNumber != value)
                {
                    _processNumber = value;
                    RaisePropertyChanged();
                }
            }
        }
        public System.Guid ProcessGuid
        {
            get { return _processGuid; }
            set
            {
                if (_processGuid != value)
                {
                    _processGuid = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int GroupId
        {
            get { return _groupId; }
            set
            {
                if (_groupId != value)
                {
                    _groupId = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
