using ERP.Client.Model;
using ERP.Client.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

        public ElementViewerPage()
        {
            this.InitializeComponent();

            ElementCollection = new ElementViewModel();
            DivisionCollection = new DivisionViewModel();
        }

        private void DataGridElementView_LoadingRowGroup(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            ElementModel item = group.GroupItems[0] as ElementModel;
            e.RowGroupHeader.PropertyValue = item.Position.ToString();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var elements = await Proxy.GetElements();

            foreach (var element in elements)
            {
                element.Contraction = "Bl";

                Random rnd = new Random();
                element.Amount = Math.Round(rnd.NextDouble() * element.Count);
            }

            ElementCollection.Load(elements);

            var divisions = await Proxy.GetAllDivisions();
            DivisionCollection.Load(divisions);

            DataGridElementView.ItemsSource = ElementCollection.Elements;
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
            /*if (sender is ProgressBar progressBar && progressBar.DataContext is ElementModel element)
            {
                SetProgressBarColor(progressBar, element);
            }*/
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



    }
}
