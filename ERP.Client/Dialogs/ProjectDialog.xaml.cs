using System;
using System.Collections.Generic;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class ProjectDialog : ContentDialog
    {
        public ProjectNumberViewModel ProjectNumberCollection { get; private set; }
        public PlantOrderViewModel PlantOrderCollection { get; private set; }

        public ProjectDialog()
        {
            this.InitializeComponent();

            ProjectNumberCollection = new ProjectNumberViewModel();
            PlantOrderCollection = new PlantOrderViewModel();
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var projectNumbers = await Proxy.GetProjectNumbersAsync();
            ProjectNumberCollection.Load(projectNumbers);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

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
            PlantOrderCollection.Load(plantOrders);
            PlantOrderListView.ItemsSource = PlantOrderCollection.PlantOrders;
            ProjectPivot.SelectedIndex = 1;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
