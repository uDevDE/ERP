using ERP.Client.Model;
using ERP.Client.utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace ERP.Client.ViewModel
{
    public class MaterialRequirementsViewModel
    {
        public ObservableCollection<MaterialRequirementModel> MaterialRequirements { get; private set; }
        public MaterialRequirementsViewModel() => MaterialRequirements = new ObservableCollection<MaterialRequirementModel>();

        public CollectionViewSource GroupedItems { get; private set; }

        public void Load(List<MaterialRequirementModel> materialRequirements)
        {
            MaterialRequirements.Clear();
            foreach (var materialRequirement in materialRequirements)
            {
                MaterialRequirements.Add(materialRequirement);
            }
        }

        public CollectionViewSource GroupData()
        {
            ObservableCollection<GroupInfoCollection<MaterialRequirementModel>> groups = new ObservableCollection<GroupInfoCollection<MaterialRequirementModel>>();
            var query = from item in MaterialRequirements
                        orderby item
                        group item by item.MaterialNumber into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoCollection<MaterialRequirementModel> info = new GroupInfoCollection<MaterialRequirementModel>
                {
                    Key = g.GroupName
                };

                foreach (var item in g.Items)
                {
                    info.Add(item);
                }

                groups.Add(info);
            }

            GroupedItems = new CollectionViewSource
            {
                IsSourceGrouped = true,
                Source = groups
            };

            return GroupedItems;
        }

    }
}
