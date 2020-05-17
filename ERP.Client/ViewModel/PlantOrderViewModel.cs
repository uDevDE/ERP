using ERP.Client.Model;
using ERP.Client.utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace ERP.Client.ViewModel
{
    public class PlantOrderViewModel
    {
        public ObservableCollection<PlantOrderModel> PlantOrders { get; private set; }
        public PlantOrderViewModel() => PlantOrders = new ObservableCollection<PlantOrderModel>();

        public CollectionViewSource GroupedItems { get; private set; }

        public void Load(List<PlantOrderModel> plantOrders)
        {
            PlantOrders.Clear();
            foreach (var plantOrder in plantOrders)
            {
                PlantOrders.Add(plantOrder);
            }
        }

        public CollectionViewSource GroupData()
        {
            ObservableCollection<GroupInfoCollection<PlantOrderModel>> groups = new ObservableCollection<GroupInfoCollection<PlantOrderModel>>();
            var query = from item in PlantOrders
                        orderby item
                        group item by GetPlantOrderMainNumber(item.Number) into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoCollection<PlantOrderModel> info = new GroupInfoCollection<PlantOrderModel>
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

        public static string GetPlantOrderMainNumber(string number)
        {
            var pattern = @"(\d{5}.\d{1})";
            var match = System.Text.RegularExpressions.Regex.Match(number, pattern);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

    }
}
