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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class ProjectDialog : ContentDialog
    {
        public ProjectNumberViewModel ProjectNumberCollection { get; private set; }

        public ProjectDialog()
        {
            this.InitializeComponent();

            ProjectNumberCollection = new ProjectNumberViewModel();
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

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is ProjectNumberModel projectNumber)
            {
                sender.Text = projectNumber.DisplayText;
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // Use args.QueryText to determine what to do.
                this.Title = args.QueryText;
            }
        }


    }
}
