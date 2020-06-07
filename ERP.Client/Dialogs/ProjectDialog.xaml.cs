using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Helpers;
using ERP.Client.ViewModel;
using ERP.Client.Model;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using ERP.Client.Collection;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class ProjectDialog : ContentDialog
    {
        public PlantOrderModel Result { get; private set; }
        public ProjectNumberViewModel ProjectNumberCollection { get; private set; }
        public PlantOrderViewModel PlantOrderCollection { get; private set; }
        public ObservableCollection<PlantOrderModel> FilteredPlantOrders { get; private set; }
        public ObservableCollection<PlantOrderModel> FilteredProductionOrders { get; private set; }
        public DivisionViewModel DivisionCollection { get; private set; }
        public ProductionViewCollection ProductionCollection { get; private set; }

        public ProjectDialog(DivisionViewModel collection)
        {
            this.InitializeComponent();

            ProjectNumberCollection = new ProjectNumberViewModel();
            PlantOrderCollection = new PlantOrderViewModel();
            DivisionCollection = collection;
            ProductionCollection = new ProductionViewCollection();
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var projectNumbers = await Proxy.GetProjectNumbersAsync();
            if (projectNumbers != null)
            {
                ProjectNumberCollection.Load(projectNumbers);
            }

            if (await ProductionCollection.Load())
            {
                FilteredProductionOrders = new ObservableCollection<PlantOrderModel>(ProductionCollection.PlantOrderCollection.OrderBy(x => x.Number).ToList());
                ListView.ItemsSource = FilteredProductionOrders;
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var index = MainPivot.SelectedIndex;
            var selectedItem = index == 0 ? ListView.SelectedItem : PlantOrderListView.SelectedItem;
            if (selectedItem is PlantOrderModel plantOrder)
            {
                if (index == 1)
                {
                    await ProductionCollection.Add(plantOrder);
                }

                Result = plantOrder;
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }



        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = new List<ProjectNumberModel>(from item in ProjectNumberCollection.ProjectNumbers
                                                                  where item.DisplayText.Contains(sender.Text)
                                                                  select item);
            }
        }

        private async void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is ProjectNumberModel projectNumber)
            {
                sender.Text = projectNumber.DisplayText;
                ProjectNumberCollection.SelectedProjectNumber = projectNumber;
                await LoadPlantOrders(projectNumber);
            }
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                try
                {
                    var projectNumber = ProjectNumberCollection.ProjectNumbers.Single(x => x.DisplayText == args.QueryText);
                    if (projectNumber != null)
                    {
                        ProjectNumberCollection.SelectedProjectNumber = projectNumber;
                        await LoadPlantOrders(projectNumber);
                    }

                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    var projectNumber = ProjectNumberCollection.ProjectNumbers.FirstOrDefault(x => x.DisplayText.Contains(args.QueryText));
                    if (projectNumber != null)
                    {
                        ProjectNumberCollection.SelectedProjectNumber = projectNumber;
                        sender.Text = projectNumber.DisplayText;
                        await LoadPlantOrders(projectNumber);
                    }
                }
                catch (Exception) { }
            }
        }

        private async Task LoadPlantOrders(ProjectNumberModel projectNumber)
        {
            if (projectNumber == null)
                return;
        
            var plantOrders = await Proxy.GetPlantOrders(21903);
            if (plantOrders != null)
            {
                PlantOrderCollection.Load(plantOrders.OrderBy(x => x.Number).ToList());
                FilteredPlantOrders = new ObservableCollection<PlantOrderModel>(PlantOrderCollection.PlantOrders);
                PlantOrderListView.ItemsSource = FilteredPlantOrders;
                ProjectPivot.SelectedIndex = 1;
            }
        }

        private bool Filter(PlantOrderModel plantOrder, string filter)
        {
            return plantOrder.Number?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) == true ||
                plantOrder.Name?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) == true ||
                plantOrder.Description?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) == true ||
                plantOrder.Section?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) == true;
        }

        private void Remove_NonMatching(IEnumerable<PlantOrderModel> filteredData)
        {
            for (int i = FilteredPlantOrders.Count - 1; i >= 0; i--)
            {
                var item = FilteredPlantOrders[i];
                if (!filteredData.Contains(item))
                {
                    FilteredPlantOrders.Remove(item);
                }
            }
        }

        private void AddBack_Contacts(IEnumerable<PlantOrderModel> filteredData)
        {
            foreach (var item in filteredData)
            {
                if (!FilteredPlantOrders.Contains(item))
                {
                    FilteredPlantOrders.Add(item);
                }
            }
        }

        private void PlantOrderFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = PlantOrderCollection.PlantOrders.Where(x => Filter(x, PlantOrderFilterTextBox.Text));
            Remove_NonMatching(filtered);
            AddBack_Contacts(filtered);
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = ProductionCollection.PlantOrderCollection.Where(x => Filter(x, FilterTextBox.Text));
            Remove_NonMatching(filtered);
            AddBack_Contacts(filtered);
        }

        private async void ButtonDeletePlantOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is PlantOrderModel plantOrder)
            {
                if (await ProductionCollection.Remove(plantOrder))
                {
                    FilteredProductionOrders.Remove(plantOrder);
                }
            }
        }


    }
}
