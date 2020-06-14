using ERP.Contracts.Domain.Core;
using System.ComponentModel;

namespace ERP.Client.Model
{
    public class ElementInfoModel : IElementInfo, INotifyPropertyChanged
    {
        private System.DateTime _time;
        private double _amount;

        public int ElementInfoId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    RaisePropertyChanged("Time");
                }
            }
        }
        public double Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    RaisePropertyChanged("Amount");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
