using ERP.Client.Collection;
using ERP.Client.Dialogs;
using ERP.Client.Model;
using ERP.Client.Session;
using ERP.Client.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Startup.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ElementViewerPage : Page
    {
        public ElementViewModel ElementCollection { get; private set; }
        public DivisionViewModel DivisionCollection { get; private set; }
        public ElementFilterViewModel FilterCollection { get; private set; }
        public EmployeeModel CurrentEmployee { get; private set; }
        public PlantOrderModel PlantOrder { get; private set; }

        public ElementViewerPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            ElementCollection = new ElementViewModel();
            DivisionCollection = new DivisionViewModel();
            FilterCollection = new ElementFilterViewModel();

            LoadingControl.IsLoading = true;
        }

        private void DataGridElementView_LoadingRowGroup(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            ElementModel item = group.GroupItems[0] as ElementModel;
            e.RowGroupHeader.PropertyValue = item.Position.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentEmployee = e.Parameter as EmployeeModel;

            base.OnNavigatedTo(e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var divisions = await Proxy.GetAllDivisions();
            DivisionCollection.Load(divisions);

            DataGridElementView.ItemsSource = ElementCollection.Elements;

            var sessionExists = await ElementViewSession.FileExistsAsync();

            if (!sessionExists)
            {
                await ShowProjectDialog();
            }
            else
            {
                LoadingText.Text = "Die letzte Sitzung wird wieder hergestellt";
                var session = await ElementViewSession.LoadAsync();
                if (session == null || session.Divison == null || session.PlantOder == null)
                {
                    await ShowProjectDialog();
                }
                else
                {
                    await GetElements(session.PlantOder, session.Divison);
                }
            }

            LoadingControl.IsLoading = false;
        }

        public async Task ShowProjectDialog()
        {
            var dialog = new ProjectDialog(DivisionCollection);
            var dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                if (dialog.Result == null || dialog.Division == null)
                {
                    var infoDialog = new InfoDialog("Einige Daten wurden nicht gefunden!", "Fehler", Core.Enums.InfoDialogType.Error);
                    await infoDialog.ShowAsync();
                }
                //await GetElements();
                await GetElements(dialog.Result, dialog.Division);
            }
        }

        private async Task GetElements(PlantOrderModel plantOrder, DivisionModel division)
        {
            PlantOrder = plantOrder;
            List<ElementModel> elements;

            var filters = await Proxy.GetFiltersAsync(plantOrder.Id, CurrentEmployee.EmployeeId);
            FilterCollection.Load(filters);

            if (division.DivisionType.DivisionType == Contracts.Domain.Core.Enums.DivisionType.Profile)
            {
                elements = await Proxy.GetElementProfiles(plantOrder.Id, division.DivisionId);
                DataGridElementView.Columns[2].Header = "Profil";
            }
            else
            {
                elements = await Proxy.GetElements();
                DataGridElementView.Columns[2].Header = "Position";
                foreach (var element in elements)
                {
                    element.Contraction = (element.Count % 2 == 0) ? "Bl" : "Fl";

                    Random rnd = new Random();
                    element.Amount = Math.Round(rnd.NextDouble() * element.Count);
                }
            }

            for (int i = 0; i < DivisionComboBox.Items.Count; i++)
            {
                if (DivisionComboBox.Items[i] is DivisionModel item)
                {
                    if (item.DivisionId == division.DivisionId)
                    {
                        DivisionComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }

            PlantOrderHeaderText.Text = string.Format("{0:s} - {1:s} - {2:s} - {3:s}", plantOrder.Number, plantOrder.Name, plantOrder.Section, plantOrder.Description);
            LastRefreshText.Text = DateTime.Now.ToString("hh:mm:ss - ddd d MMM", CultureInfo.CreateSpecificCulture("de-DE"));

            var session = new ElementViewSession() { Divison = division, PlantOder = plantOrder };
            if (await ElementViewSession.SaveAsync(session) == false)
            {
                NotificationControl.Show("Die aktuelle Sitzung konnte nicht gespeichert werden", 4000);
            }

            ElementCollection.Load(elements);
        }

        public static SolidColorBrush PercentToColor(double percent)
        {
            if (percent < 0 || percent > 1) { return new SolidColorBrush(Colors.Transparent); }

            byte r, g;
            if (percent < 0.5)
            {
                r = 255;
                g = Convert.ToByte((255 * percent / 0.5));
            }
            else
            {
                g = 255;
                r = Convert.ToByte(255 - (255 * (percent - 0.5) / 0.5));
            }
            return new SolidColorBrush(Color.FromArgb(180, r, g, 10));
        }

        private void SetProgressBarColor(ProgressBar progressBar, ElementModel element)
        {
            var percent = (((element.Amount * 100) / element.Count) / 100);
            progressBar.Foreground = PercentToColor(percent);
        }

        private void ProgressBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is ProgressBar progressBar && progressBar.DataContext is ElementModel element)
            {
                SetProgressBarColor(progressBar, element);
            }
        }

        private void ProgressBar_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
        {
            if (sender is ProgressBar progressBar && progressBar.DataContext is ElementModel element)
            {
                SetProgressBarColor(progressBar, element);
            }
        }

        private void MenuItemGroupData_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.Tag is string propertyName)
            {
                DataGridElementView.RowGroupHeaderPropertyNameAlternative = item.Text;
                DataGridElementView.ItemsSource = ElementCollection.GroupData(propertyName).View;
            }
        }

        private void DataGridElementView_Sorting(object sender, DataGridColumnEventArgs e)
        {
            var propertyName = e.Column.Tag?.ToString();
            if (string.IsNullOrEmpty(propertyName))
                return;

            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         orderby item.GetType().GetProperty(propertyName).GetValue(item, null) descending
                                                                                         select item);
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         orderby item.GetType().GetProperty(propertyName).GetValue(item, null) ascending
                                                                                         select item);
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }

            foreach (var column in DataGridElementView.Columns)
            {
                if (column.Tag?.ToString() != propertyName)
                {
                    column.SortDirection = null;
                }
            }
        }

        private async void ButtonCreateFilter_Click(object sender, RoutedEventArgs e)
        {
            var filter = TextBoxFilter.Text;
            if (string.IsNullOrWhiteSpace(filter))
            {
                var infoDialog = new InfoDialog("Das Feld für den gesuchten Wert muss ausgefüllt sein!");
                await infoDialog.ShowAsync();
                return;
            }

            var yesNoDialog = new YesNoDialog("Filter speichern?", "Soll der Filter für dieses Projekt gespeichert werden?" + Environment.NewLine + Environment.NewLine +
                "[Ja]              Der Filter wird gespeichert, wird unter 'Bekannte Filter' hinzugefügt und ist für dieses Projekt jeder Zeit abrufbar" + Environment.NewLine + Environment.NewLine +
                "[Nein]          Der Filter wird unter 'Bekannte Filter' hinzugefügt, ist jedoch nach Beenden dieser Sitzung nicht mehr verfügbar" + Environment.NewLine +
                Environment.NewLine +
                "[Abbrechen] Der Filter wird nicht erstellt");
            await yesNoDialog.ShowAsync();

            if (yesNoDialog.Result != Dialogs.Core.Enums.YesNoDialogType.Abort && PlantOrder != null)
            {
                var selectedProperty = (ComboBoxFilterProperty.SelectedItem as ComboBoxItem).Tag.ToString();
                var selectedAction = (ComboBoxFilterAction.SelectedItem as ComboBoxItem).Tag.ToString();

                var elementFilter = new ElementFilterModel()
                {
                    Action = selectedAction,
                    PropertyName = selectedProperty,
                    Filter = filter,
                    EmployeeId = CurrentEmployee.EmployeeId,
                    PlantOrderId = PlantOrder.Id
                };

                FilterCollection.Add(elementFilter);
                TextBoxFilter.Text = string.Empty;
                FilterFlyout.Hide();
                Filter(selectedProperty, selectedAction, filter);
                FilterCollection.SelectedFilter = elementFilter;

                if (yesNoDialog.Result == Dialogs.Core.Enums.YesNoDialogType.Yes)
                {
                    elementFilter.FilterId = await Proxy.UpsertElementFilter(elementFilter);
                }
            }
        }

        private void Filter(string propertyName, string action, string filter)
        {
            if (action == "GreaterThen" || action == "LessThen")
            {
                if (propertyName == "Count" || propertyName == "Amount")
                {
                    if (double.TryParse(filter, out double value))
                    {
                        FilterElements(propertyName, action, value);
                        return;
                    }
                }
                if (propertyName == "Id")
                {
                    if (int.TryParse(filter, out int value))
                    {
                        FilterElements(propertyName, action, value);
                    }
                }
            }
            else
            {
                FilterElements(propertyName, action, filter);
            }
        }

        private void FilterElements(string propertyName, string action, string filter)
        {
            if (action == "Contain")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where item.GetType().GetProperty(propertyName).GetValue(item, null).ToString().Contains(filter)
                                                                                         select item);
            }
            else if (action == "Equal")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where item.GetType().GetProperty(propertyName).GetValue(item, null).ToString() == filter
                                                                                         select item);
            }
            else if (action == "NotEqual")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where item.GetType().GetProperty(propertyName).GetValue(item, null).ToString() != filter
                                                                                         select item);
            }
        }

        private void FilterElements(string propertyName, string action, double filter)
        {
            if (action == "GreaterThen")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where (item.GetType().GetProperty(propertyName).GetValue(item, null) as double?) > filter
                                                                                         select item);
            }
            else if (action == "LessThen")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where (item.GetType().GetProperty(propertyName).GetValue(item, null) as double?) < filter
                                                                                         select item);
            }
        }

        private void FilterElements(string propertyName, string action, int filter)
        {
            if (action == "GreaterThen")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where (item.GetType().GetProperty(propertyName).GetValue(item, null) as int?) > filter
                                                                                         select item);
            }
            else if (action == "LessThen")
            {
                DataGridElementView.ItemsSource = new ObservableCollection<ElementModel>(from item in ElementCollection.Elements
                                                                                         where (item.GetType().GetProperty(propertyName).GetValue(item, null) as int?) < filter
                                                                                         select item);
            }
        }

        private void ButtonRemoveFilter_Click(object sender, RoutedEventArgs e)
        {
            DataGridElementView.ItemsSource = ElementCollection.Elements;
            FilterCollection.SelectedFilter = null;
        }

        private async void ButtonDeleteFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ElementFilterModel filter)
            {
                var yesNoDialog = new YesNoDialog("Filter löschen?", "Soll der Filter " + Environment.NewLine + Environment.NewLine +
                    $"Eigenschaft: {filter.PropertyText}" + Environment.NewLine +
                    $"Aktion: {filter.ActionText}" + Environment.NewLine +
                    $"Filter: {filter.Filter}" + Environment.NewLine + Environment.NewLine +
                    "wirklich gelöscht werden?");
                await yesNoDialog.ShowAsync();
                if (yesNoDialog.Result == Dialogs.Core.Enums.YesNoDialogType.Yes)
                {

                }
            }
        }

        private void FilterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ElementFilterModel filter)
            {
                Filter(filter.PropertyName, filter.Action, filter.Filter);
                FilterCollection.SelectedFilter = filter;
                FilterFlyout.Hide();
            }
        }

        private async void ButtonOpenProject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProjectDialog(DivisionCollection);
            var dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                //await GetElements();
            }
        }

        private void SaveProject(PlantOrderModel plantOrder, DivisionModel division)
        {

        }

        private async void DivisionComboBox_DropDownClosed(object sender, object e)
        {
            var selectedItem = DivisionComboBox.SelectedItem;
            if (selectedItem is DivisionModel division)
            {
                var session = new ProductionViewCollection();
                if (await session.Load())
                {
                    await session.Update(PlantOrder, division);
                }

                await GetElements(PlantOrder, division);
            }
        }

        private async void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            if (PlantOrder != null && DivisionComboBox.SelectedItem is DivisionModel division)
            {
                await GetElements(PlantOrder, division);
                NotificationControl.Show($"{PlantOrder.Number} - {PlantOrder.Name} - {PlantOrder.Section} wurde aktualisiert", 3500);
            }
        }


    }
}
