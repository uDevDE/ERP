using ERP.Client.Core.Enums;
using ERP.Client.Dialogs;
using ERP.Client.Lib;
using ERP.Client.Mapper;
using ERP.Client.Model;
using ERP.Client.Startup.Resolver;
using ERP.Client.ViewModel;
using ERP.Client.ViewModel.PdfViewer;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ProjectContentPage : Page
    {
        public delegate void PdfViewerButtonToggledClickedHandler(PdfViewerPage pdfViewer, PdfFileModel pdfFile);
        public event PdfViewerButtonToggledClickedHandler PdfViewerButtonToggledClicked;

        public List<TabViewItem> TabViewItems { get; private set; }

        public bool PageLoaded { get; private set; }
        public string RemoteRootPath { get; private set; }

        public ProjectPreviewType ProjectPreviewType { get; private set; }
        public FolderModel Folder { get; private set; }
        public FileEntryModel FileEntry { get; private set; }
        public PlantOrderModel PlantOrder { get; private set; }

        public MaterialRequirementsViewModel MaterialRequirementsViewModel { get; set; }
        public ElementViewModel ElementViewModel { get; set; }
        public ObservableCollection<ProcessQRImageModel> ProcessQRImages { get; private set; }
        public PdfPageViewModel PageViewModel { get; set; }

        public static readonly Color ColorGreen = Color.FromArgb(255, 0, 179, 62);
        public static readonly Color ColorRed = Color.FromArgb(255, 184, 0, 12);

        public ProjectContentPage()
        {
            this.InitializeComponent();

            MaterialRequirementsViewModel = new MaterialRequirementsViewModel();
            ElementViewModel = new ElementViewModel();
            ProcessQRImages = new ObservableCollection<ProcessQRImageModel>();
            LoadingControl.IsLoading = true;

            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            object[] args = e.Parameter as object[];

            if (args.Length == 4)
            {
                if (args[0] is ProjectPreviewType type && args[1] is FolderModel folder)
                {
                    ProjectPreviewType = type;
                    Folder = folder;
                    if (args[2] is FileEntryModel fileEntry)
                    {
                        FileEntry = fileEntry;
                    }

                    if (args[3] is PlantOrderModel plantOrder)
                    {
                        PlantOrder = plantOrder;
                    }
                }
            }


            base.OnNavigatedTo(e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (PageLoaded)
                return;

            if (FileEntry == null && PlantOrder == null)
            {
                var dialog = new MessageDialog("Da ist wohl etwas schief gelaufen");
                await dialog.ShowAsync();
                LoadingControl.IsLoading = false;
                return;
            }

            if (FileEntry == null && PlantOrder != null)
            {
                var infoDialog = new InfoDialog("Fehler beim Bestimmen der Datei oder des Werkauftrags!", "Information", InfoDialogType.Error);
                await infoDialog.ShowAsync();
            }

            RemoteRootPath = await Proxy.GetRemoteRootPath();
            if (string.IsNullOrEmpty(RemoteRootPath))
            {
                var infoDialog = new InfoDialog("Beim Abfragen des Stammverzeichnisses ist etwas schief gelaufen!", "Information", InfoDialogType.Error);
                await infoDialog.ShowAsync();
            }

            var plantOrderProcesses = await Proxy.GetPlantOrderProcesses(PlantOrder.Id);
            foreach (var plantOrderProcess in plantOrderProcesses)
            {
                ProcessQRImages.Add(new ProcessQRImageModel()
                {
                    ProcessName = plantOrderProcess.Process,
                    Source = await QRCodeImageGenerator.Generate(plantOrderProcess.ProcessGuid.ToString())
                });
            }

            /*var materialRequirements = await Proxy.GetMaterialRequirements(new string[] { "1", "2" });
            if (materialRequirements != null)
            {
                MaterialRequirementsViewModel.Load(materialRequirements);
                DataGridMaterialRequirements.ItemsSource = MaterialRequirementsViewModel.GroupData().View;
            }*/

            var elements = await Proxy.GetElements();
            if (elements != null)
            {
                ElementViewModel.Load(elements);
            }

            var profiles = await Proxy.GetProfiles(PlantOrder.Id);
            if (profiles != null)
            {
                ElementViewModel.Load(profiles);
                var d = new MessageDialog(profiles.Count.ToString());
                await d.ShowAsync();
            }

            if (!PageLoaded)
            {
                var fullFilePath = Path.Combine(RemoteRootPath, FileEntry.FilePath).Replace("/", @"\");

                var remoteFile = await StorageFile.GetFileFromPathAsync(fullFilePath);
                if (remoteFile != null)
                {
                    var localFile = await CopyFileToDeviceAsync(remoteFile);
                    StreamUriWinRTResolver myResolver = new StreamUriWinRTResolver();
                    PdfViewerControl.NavigateToLocalStreamUri(new Uri(localFile.Path), myResolver);
                }
            }

            ElementView.ItemsSource = ElementViewModel.Elements;

            LoadingControl.IsLoading = false;
            PageLoaded = true;
        }

        private void DataGridMaterialRequirements_LoadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            MaterialRequirementModel item = group.GroupItems[0] as MaterialRequirementModel;
            e.RowGroupHeader.PropertyValue = item.MaterialNumber.ToString();
        }

        private async void ButtonNewPivotItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FileSelectionDialog(Folder);
            await dialog.ShowAsync();
        }

        private void ProcessToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var isOn = (sender as ToggleSwitch).IsOn;

            if (isOn == true)
            {
                QRCodeFlipView.Visibility = Visibility.Collapsed;
                QRCodeGridView.Visibility = Visibility.Visible;
            }
            else
            {
                QRCodeGridView.Visibility = Visibility.Collapsed;
                QRCodeFlipView.Visibility = Visibility.Visible;
            }
        }

        private IAsyncOperation<StorageFile> CopyFileToDeviceAsync(StorageFile file, NameCollisionOption options = NameCollisionOption.ReplaceExisting)
        {
            var tempFolder = ApplicationData.Current.TemporaryFolder;
            return file.CopyAsync(tempFolder, file.Name, options);
        }

        private void LoadPdfViewer(StorageFile file, PdfFileModel pdfFile)
        {
            PivotItem newItem = new PivotItem
            {
                Header = file.DisplayName,
                //IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document }
            };

            Frame frame = new Frame();

            frame.Navigated += Frame_Navigated;
            frame.Navigate(typeof(PdfViewerPage), new object[] { file, pdfFile, true });
            newItem.Content = frame;

            //PivotControl.Items.Insert(PivotControl.Items.Count - 1, newItem);
            //PivotControl.SelectedItem = newItem;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is PdfViewerPage pdfViewer)
            {
                pdfViewer.ButtonCloseClicked += OnPdfViewerCloseButtonClicked;
                pdfViewer.ButtonFavoriteToggled += OnButtonFavoriteToggled;
            }
        }

        private void OnButtonFavoriteToggled(PdfViewerPage pdfViewer, PdfFileModel pdfFile)
        {
            PdfViewerButtonToggledClicked?.Invoke(pdfViewer, pdfFile);
        }

        private void OnPdfViewerCloseButtonClicked(PdfViewerPage pdfViewer)
        {
            //var item = PivotControl.Items.Single(x => (((x as PivotItem).Content as Frame).Content as PdfViewerPage) == pdfViewer);
            //PivotControl.Items.Remove(item as PivotItem);
        }

        private void ElementProgressBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is ProgressBar progressBar && progressBar.DataContext is ElementModel element)
            {
                var percent = (((element.Amount * 100) / element.Count) / 100);
                progressBar.Foreground = PercentToColor(percent);
            }
        }

        private void ElementView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedElement = ElementView.SelectedItem;
            if (selectedElement is ElementModel element)
            {
                ElementViewModel.SelectedElement = element;
            }
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

        private async void ButtonComment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ElementModel element)
            {
                var dialog = new MessageDialog(element.Position);
                await dialog.ShowAsync();
            }
        }

        private async void ButtonMore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ElementModel element)
            {
                var dialog = new MessageDialog(element.Position);
                await dialog.ShowAsync();
            }
        }

        private void NumberBox_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            if (ElementViewModel.SelectedElement != null)
            {
                ElementViewModel.SelectedElement.Amount = sender.Value;
            }
        }

        private async void ButtonAddProfile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProfileDialog();
            var dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                var profile = dialog.Result;
                if (profile != null)
                {
                    if (PlantOrder != null)
                    {
                        profile.PlantOrderId = PlantOrder.Id;
                    }

                    if (FileEntry != null)
                    {
                        profile.Filename = FileEntry.Name;
                    }

                    var profileId = await Proxy.CreateProfile(profile);
                    if (profileId > 0)
                    {
                        profile.ProfileId = profileId;
                        var element = AutoMapperConfiguration.Map(profile);
                        ElementViewModel.Add(element);
                    }
                    else
                    {
                        var errorDialog = new InfoDialog("Beim Anlegen des Profil's ist ein Fehler aufgetreten!", "Information", InfoDialogType.Error);
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
