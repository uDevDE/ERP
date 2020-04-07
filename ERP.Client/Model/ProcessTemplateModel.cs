using ERP.Contracts.Domain.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class ProcessTemplateModel : INotifyPropertyChanged, IProcessTemplate
    {
        private string _processTemplateName;
        private string _process;
        private string _description;
        private int _processNumber;
        private string _av;
        private int _groupId;

        public string ProcessTemplateName
        {
            get { return _processTemplateName; }
            set
            {
                if (_processTemplateName != value)
                {
                    _processTemplateName = value;
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
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
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
        public string AV
        {
            get { return _av; }
            set
            {
                if (_av != value)
                {
                    _av = value;
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
