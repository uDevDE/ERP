using ERP.Client.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ERP.Client.ViewModel
{
    public class MaterialRequirementsViewModel
    {
        public ObservableCollection<MaterialRequirementModel> MaterialRequirements { get; set; }
        public MaterialRequirementsViewModel() => MaterialRequirements = new ObservableCollection<MaterialRequirementModel>();

        public void Load(List<MaterialRequirementModel> materialRequirements)
        {
            MaterialRequirements.Clear();
            foreach (var materialRequirement in materialRequirements)
            {
                MaterialRequirements.Add(materialRequirement);
            }
        }
    }
}
