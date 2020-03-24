using Microsoft.Toolkit.Uwp.UI.Controls;
using ERP.Client.Core;
using ERP.Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PdfViewerPreviewPage : Page
    {
        public ObservableCollection<FolderModel> Projects { get; set; }
        public ObservableCollection<PlantOrderModel> PlantOrders { get; set; }
        public PdfDocument PdfDocument { get; private set; }
        private int _employeeId;
        private LocalUserSettings _userSettings;

        public PdfViewerPreviewPage()
        {
            this.InitializeComponent();

            Projects = new ObservableCollection<FolderModel>();
            PlantOrders = new ObservableCollection<PlantOrderModel>();
            LoadUserSettings();
        }

        private void LoadUserSettings()
        {
            var employeeId = LocalClient.EmployeeViewModel?.Employee.EmployeeId;
            if (employeeId.HasValue)
            {
                _employeeId = employeeId.Value;
                var settings = LocalUserSettings.Load(_employeeId);
                if (settings != null)
                {
                    _userSettings = settings;
                    return;
                }
            }

            _userSettings = new LocalUserSettings();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Load(Dictionary<string, FolderModel> projects)
        {
            Projects.Clear();
            foreach (var value in projects.Values)
            {
                Projects.Add(value);
            }
        }

        private async void MasterDetailsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is MasterDetailsView masterDetailsView)
            {
                if (masterDetailsView.SelectedItem is FolderModel folder)
                {
                    var number = System.Text.RegularExpressions.Regex.Split(folder.Name, @"\D+").FirstOrDefault().Trim();
                    if (int.TryParse(number, out int plantOrderNumber))
                    {
                        var plantOrders = await Proxy.GetPlantOrders(plantOrderNumber);

                        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
                        {
                            foreach (var plantOrder in plantOrders)
                            {
                                PlantOrders.Add(plantOrder);
                            }
                        });
                    }

                    if (_employeeId > 0)
                    {
                        _userSettings.LastSelectedProjectDir = folder.Name;
                        LocalUserSettings.Save(_employeeId, _userSettings);
                    }
                }
            }
        }

        private void ComboBoxWorkDir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.Parent is RelativePanel panel && panel.Children[2] is ListBox listBox)
                {
                    if ((sender as ComboBox).SelectedItem is FolderModel folder)
                    {
                        listBox.ItemsSource = folder.Files?.Values.ToList();
                    }
                }
            }
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem is FileEntryModel fileEntry)
                {
                    var filePath = Path.Combine(fileEntry.RelativePath, fileEntry.Name + ".pdf");
                    var pdfFile = await Proxy.GetPdfFile(filePath);
                    if (pdfFile?.Buffer?.Length > 0)
                    {
                        LoadPreviewPage(pdfFile.Buffer);
                    }
                }
            }
        }

        private async void LoadPreviewPage(byte[] arr)
        {
            var stream = arr.AsBuffer().AsStream().AsRandomAccessStream();
            PdfDocument = await PdfDocument.LoadFromStreamAsync(stream);
            if (PdfDocument.PageCount > 0)
            {
                PdfViewerPreview.Page = PdfDocument.GetPage(0);
            }
        }

        private void MasterDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_userSettings.LastSelectedProjectDir))
            {
                if (sender is MasterDetailsView masterDetailsView)
                {
                    foreach (var item in masterDetailsView.Items)
                    {
                        if (item is FolderModel folder)
                        {
                            if (folder.Name == _userSettings.LastSelectedProjectDir)
                            {
                                masterDetailsView.SelectedItem = item;
                            }
                        }
                    }
                }
            }
        }

        private void ComboBoxWorkDir_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (sender is ComboBox comboBox)
            {
                foreach (var item in comboBox.Items)
                {
                    if (item is FolderModel folder)
                    {
                        if (folder.Name == "12 - Fertigungsaufträge")
                        {
                            comboBox.SelectedItem = item;
                        }
                    }
                }
            }
        }


    }
}
