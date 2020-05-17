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
using ERP.Client.Core.Enums;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ERP.Client.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Startup.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectPreviewPage : Page
    {
        public delegate void ButtonOpenProjectClickedHandler(ProjectPreviewPage projectPreviewPage, ProjectPreviewType projectPreviewType, FolderModel project);
        public event ButtonOpenProjectClickedHandler ButtonOpenProjectClicked;

        public int Index { get; private set; }
        public ObservableCollection<FolderModel> Projects { get; set; }
        public ObservableCollection<PlantOrderModel> PlantOrders { get; set; }
        public PdfDocument PdfDocument { get; private set; }
        private int _employeeId;
        private LocalUserSettings _userSettings;
        private ProjectPreviewType _currentProjectPreviewType;

        public FileEntryModel SelectedFileEntry { get; private set; }
        public PlantOrderModel SelectedPlantOrder { get; private set; }
        public FolderModel SelectedProject { get; private set; }

        public PlantOrderViewModel CurrentPlantOrders { get; private set; }
        public List<FileEntryModel> CurrentFileEntries { get; private set; }

        private const string BUTTON_LABEL_PLANT_ORDER = "Von Werkauftrag öffnen";
        private const string BUTTON_LABEL_FILE_ENTRY = "Von Datei öffnen";
        

        public ProjectPreviewPage()
        {
            this.InitializeComponent();

            Projects = new ObservableCollection<FolderModel>();
            PlantOrders = new ObservableCollection<PlantOrderModel>();

            CurrentPlantOrders = new PlantOrderViewModel();
            CurrentFileEntries = new List<FileEntryModel>();

            LoadUserSettings();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is int index)
            {
                Index = index;
            }

            base.OnNavigatedTo(e);
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


        private void ToggleAppBarButtons(bool enable)
        {
            if (ButtonOpenProject.IsEnabled == !enable) ButtonOpenProject.IsEnabled = enable;
            if (ButtonFavorite.IsEnabled == !enable) ButtonFavorite.IsEnabled = enable;
            if (ButtonRefresh.IsEnabled == !enable) ButtonRefresh.IsEnabled = enable;
        }

        private void UpdateAppBarButtons()
        {
            if (_currentProjectPreviewType == ProjectPreviewType.PdfFile)
            {
                if (SelectedFileEntry != null)
                {
                    ToggleAppBarButtons(true);
                }
                else
                {
                    ToggleAppBarButtons(false);
                }
            }
            else if (_currentProjectPreviewType == ProjectPreviewType.PlantOrder)
            {
                if (SelectedPlantOrder != null)
                {
                    ToggleAppBarButtons(true);
                }
                else
                {
                    ToggleAppBarButtons(false);
                }
            }
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
                    SelectedProject = folder;
                    SelectedFileEntry = null;
                    SelectedPlantOrder = null;
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
                            CurrentPlantOrders.Load(plantOrders);
                            DataGridPlantOrder.ItemsSource = CurrentPlantOrders.GroupData().View;
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
                        var fileEntries = folder.Files?.Values.ToList();
                        listBox.ItemsSource = fileEntries;
                        CurrentFileEntries = fileEntries;
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
                        SelectedFileEntry = fileEntry;
                        _currentProjectPreviewType = ProjectPreviewType.PdfFile;
                        ButtonOpenProject.Label = BUTTON_LABEL_FILE_ENTRY;
                        UpdateAppBarButtons();
                        LoadPreviewPage(pdfFile.Buffer);
                    }
                }
                else
                {
                    SelectedFileEntry = null;
                }
            }
        }

        private async void LoadPreviewPage(byte[] arr)
        {
            var stream = arr.AsBuffer().AsStream().AsRandomAccessStream();
            PdfDocument = await PdfDocument.LoadFromStreamAsync(stream);
            if (PdfDocument.PageCount > 0)
            {
                //PdfViewerPreview.Page = PdfDocument.GetPage(0);
                var page = PdfDocument.GetPage(0);
                if (page != null)
                {
                    await PdfViewerPreview.Load(page);
                }
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

        private void ButtonOpenProject_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
            {
                ButtonOpenProjectClicked?.Invoke(this, _currentProjectPreviewType, SelectedProject);
            }
        }

        private void ProjectPreviewPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Pivot pivot)
            {
               if (pivot.SelectedItem is PivotItem pivotItem)
                {
                    if (pivotItem.Tag is string tag)
                    {
                        if (tag == "po")
                        {
                            _currentProjectPreviewType = ProjectPreviewType.PlantOrder;
                            ButtonOpenProject.Label = BUTTON_LABEL_PLANT_ORDER;
                        }
                        else if (tag == "pdf")
                        {
                            _currentProjectPreviewType = ProjectPreviewType.PdfFile;
                            ButtonOpenProject.Label = BUTTON_LABEL_FILE_ENTRY;
                        }
                    }
                }

                UpdateAppBarButtons();
            }
        }

        private void DataGridPlantOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is PlantOrderModel plantOrder)
                {
                    SelectedPlantOrder = plantOrder;
                    _currentProjectPreviewType = ProjectPreviewType.PlantOrder;
                    UpdateAppBarButtons();
                    ButtonOpenProject.Label = BUTTON_LABEL_PLANT_ORDER;
                }
                else
                {
                    SelectedPlantOrder = null;
                }
            }
        }

        public FileEntryModel FindFilename(PlantOrderModel plantOrder)
        {
            var projectIdentifier = plantOrder.Number.Replace('.', '-').Trim();

            foreach (var folder in SelectedProject.SubFolders)
            {
                foreach (var entry in folder.Value.Files)
                {
                    var fileInfo = entry.Value.FileInfo;
                    if (fileInfo != null)
                    {
                        if (projectIdentifier == fileInfo.ProjectIdentifier)
                        {
                            return entry.Value;
                        }
                    }
                }
            }

            return null;
        }

        private void DataGridPlantOrder_LoadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            PlantOrderModel item = group.GroupItems[0] as PlantOrderModel;
            e.RowGroupHeader.PropertyValue = PlantOrderViewModel.GetPlantOrderMainNumber(item.Number);
        }
    }


}
