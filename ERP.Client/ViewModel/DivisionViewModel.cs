using ERP.Client.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ERP.Client.ViewModel
{
    public class DivisionViewModel
    {
        public ObservableCollection<DivisionModel> Divisions { get; private set; }

        public DivisionViewModel() => Divisions = new ObservableCollection<DivisionModel>();

        public void Load(List<DivisionModel> divisions)
        {
            Divisions.Clear();
            foreach (var division in divisions)
            {
                Divisions.Add(division);
            }
        }
    }
}
