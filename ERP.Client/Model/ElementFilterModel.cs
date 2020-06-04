using ERP.Contracts.Domain.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class ElementFilterModel : IElementFilter, INotifyPropertyChanged
    {
        private string _propertyName;
        private string _action;
        private string _filter;
        private int _usedCounter;

        public int FilterId { get; set; }
        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (_propertyName != value)
                {
                    _propertyName = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("PropertyText");
                }
            }
        }
        public string Action
        {
            get { return _action; }
            set
            {
                if (_action != value)
                {
                    _action = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("ActionText");
                }
            }
        }
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int UsedCounter
        {
            get { return _usedCounter; }
            set
            {
                if (_usedCounter != value)
                {
                    _usedCounter = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int PlantOrderId { get; set; }
        public int EmployeeId { get; set; }

        public string ActionText
        {
            get
            {
                switch (_action)
                {
                    case "Equal": default: return "Wert muss gleich sein";
                    case "Contain": return "Eigentschaft muss Wert beinhalten";
                    case "NotEqual": return "Wert muss ungleich sein";
                    case "GreaterThen": return "Wert muss größer sein (Nur Zahlen)";
                    case "LessThen": return "Wert muss kleiner sein (Nur Zahlen)";
                }
            }
        }

        public string PropertyText
        {
            get
            {
                switch (_propertyName)
                {
                    case "Id": default: return "Id";
                    case "Contraction": return "Kürzel";
                    case "Position": return "Position";
                    case "Description": return "Beschreibung";
                    case "Length": return "Länge";
                    case "Amount": return "Ist";
                    case "Count": return "Soll";
                    case "Unit": return "Einheit";
                    case "Surface": return "Oberfläche";
                    case "ColourInside": return "Farbe (innen)";
                    case "ColourOutside": return "Farbe (außen)";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
