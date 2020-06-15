using ERP.Client.Core.Enums;
using ERP.Client.Dialogs;
using ERP.Client.Dialogs.Core.Enums;
using ERP.Client.Lib;
using ERP.Client.Mapper;
using ERP.Client.Model;
using ERP.Client.Startup.PdfViewer;
using ERP.Client.Startup.Resolver;
using ERP.Client.ViewModel;
using ERP.Client.ViewModel.PdfViewer;
using ERP.Contracts.Domain;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
        private bool _valueChanged;

        public delegate void PdfViewerButtonToggledClickedHandler(PdfViewerPage pdfViewer, PdfFileModel pdfFile);
        public event PdfViewerButtonToggledClickedHandler PdfViewerButtonToggledClicked;

        public List<TabViewItem> TabViewItems { get; private set; }

        public bool PageLoaded { get; private set; }
        public string RemoteRootPath { get; private set; }

        public ProjectPreviewType ProjectPreviewType { get; private set; }
        public FolderModel Folder { get; private set; }
        public FileEntryModel FileEntry { get; private set; }
        public PlantOrderModel PlantOrder { get; private set; }
        public StorageFile LocalFile { get; private set; }

        public MaterialRequirementsViewModel MaterialRequirementsViewModel { get; set; }
        public ElementViewModel ElementCollection { get; set; }
        public ObservableCollection<ProcessQRImageModel> ProcessQRImages { get; private set; }
        public PdfPageViewModel PageViewModel { get; set; }

        public static readonly Color ColorGreen = Color.FromArgb(255, 0, 179, 62);
        public static readonly Color ColorRed = Color.FromArgb(255, 184, 0, 12);

        public ProjectContentPage()
        {
            this.InitializeComponent();

            PdfViewerControl.Settings.IsJavaScriptEnabled = true;

            MaterialRequirementsViewModel = new MaterialRequirementsViewModel();
            ElementCollection = new ElementViewModel();
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

            var materialRequirements = await Proxy.GetMaterialRequirements(new string[] { "1", "2" });
            if (materialRequirements != null)
            {
                MaterialRequirementsViewModel.Load(materialRequirements);
                DataGridMaterialRequirements.ItemsSource = MaterialRequirementsViewModel.GroupData().View;
            }

            var elements = await Proxy.GetElements();
            if (elements != null)
            {
                ElementCollection.Load(elements);
            }

            var profiles = await Proxy.GetProfiles(PlantOrder.Id);
            if (profiles != null)
            {
                ElementCollection.Load(profiles);
            }

            ElementView.ItemsSource = ElementCollection.Elements;

            //PdfViewerFrame.Content = new PdfWebViewContentPage();

            if (!PageLoaded)
            {
                var fullFilePath = Path.Combine(RemoteRootPath, FileEntry.FilePath).Replace("/", @"\");

                var remoteFile = await StorageFile.GetFileFromPathAsync(fullFilePath);
                if (remoteFile != null)
                {
                    LocalFile = await CopyFileToDeviceAsync(remoteFile);

                    /*var pdfFile = new PdfFileModel()
                    {
                        FullFilePath = localFile.Path,
                        IsFavorite = false,
                        LastTimeOpened = DateTime.Now
                    };
                    LoadPdfViewer(localFile, pdfFile);*/

                    //Uri url = PdfViewerControl.BuildLocalStreamUri("MyTag", "/Assets/PdfViewer/web/viewer.html");
                    //StreamUriWinRTResolver resolver = new StreamUriWinRTResolver();
                    //PdfViewerControl.NavigateToLocalStreamUri(url, resolver);
                    //
                    //PdfViewerControl.NavigateToLocalStreamUri(new Uri(LocalFile.Path), resolver);

                    /*var url = new Uri(
                        string.Format("ms-appx-web:///Assets/PdfViewer/web/viewer.html?file={0}",
                        string.Format("ms-appx-web:///Assets/{0}", WebUtility.UrlEncode("compressed.tracemonkey-pldi-09.pdf"))));

                    var dialog = new MessageDialog(url.AbsoluteUri);
                    await dialog.ShowAsync();
                    PdfViewerControl.Source = url;*/
                }
            }
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
                ElementCollection.SelectedElement = element;
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

        /*private void NumberBox_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            if (_valueChanged)
            {
                _valueChanged = false;
                return;
            }

            if (ElementCollection.SelectedElement != null)
            {
                ElementCollection.SelectedElement.Amount = args.NewValue;
            }

            sender.Focus(FocusState.Programmatic);
        }*/

        private async void ButtonAddProfile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProfileDialog();
            var dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                var profile = dialog.Result;
                if (profile != null)
                {
                    var existingElement = ProfileExists(profile);
                    if (existingElement != null)
                    {
                        var yesNoDialog = new YesNoDialog("Profil bereits vorhanden",
                            string.Format("Das Profil mit der Profilnummer '{0:s}', einer Länge von '{1:s}' und der Farbe '{2:s}' ist bereits vorhanden. " +
                            "Soll das vorhandene Profil geändert werden?", profile.ProfileNumber, profile.Length, profile.Surface));
                        await yesNoDialog.ShowAsync();
                        if (yesNoDialog.Result == YesNoDialogType.Yes)
                        {
                            existingElement.Count += profile.Count;
                            existingElement.Amount += profile.Amount;
                            await UpdateProfile(existingElement);
                        }

                        return;
                    }

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
                        ElementCollection.Add(element);
                    }
                    else
                    {
                        var errorDialog = new InfoDialog("Beim Anlegen des Profil's ist ein Fehler aufgetreten!", "Information", InfoDialogType.Error);
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private ElementModel ProfileExists(ProfileDTO profile) =>
            ElementCollection.Find(profile.ProfileNumber, profile.Length, profile.Surface);

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ElementModel element)
            {
                var yesNoDialog = new YesNoDialog("Profil löschen?", 
                    string.Format("Bist Du sicher, dass das Profil mit der Profilnummer '{0:s}' gelöscht werden soll?", element.Position));
                await yesNoDialog.ShowAsync();

                if (yesNoDialog.Result == YesNoDialogType.Yes)
                {
                    var result = await Proxy.DeleteProfileAsync(element.Id);
                    if (!result)
                    {
                        var dialog = new InfoDialog(string.Format("Das Profil mit der Profilnummer '{0:s}' konnte nicht gelöscht werden!", element.Position));
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        ElementCollection.Remove(element);
                    }
                }
            }
        }

        private async void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ElementModel element)
            {
                await UpdateProfile(element);
            }
        }

        private async Task UpdateProfile(ElementModel element)
        {
            var dialog = new ProfileDialog(element);
            var dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary && dialog.Mode == ProfileDialogMode.Edit)
            {
                var profile = dialog.Result;
                var result = await Proxy.UpdateProfileAsync(profile);
                if (result)
                {
                    UpdateElement(element, profile);
                }
                else
                {
                    var infoDialog = new InfoDialog("Das Profil konnte nicht geändert werden!");
                    await infoDialog.ShowAsync();
                }
            }
        }

        private void UpdateElement(ElementModel element, ProfileDTO profile)
        {
            element.Position = profile.ProfileNumber;
            element.Amount = profile.Amount;
            element.Contraction = profile.Contraction;
            element.Count = profile.Count;
            element.Description = profile.Description;
            element.Length = profile.Length;
            element.Surface = profile.Surface;
        }

        private void PdfViewerControl_LoadCompleted(object sender, NavigationEventArgs e)
        {
            LoadingControl.IsLoading = false;
            PageLoaded = true;
        }

        private async Task<string> OpenAndConvert(StorageFile file)
        {
            var filebuffer = await file.OpenAsync(FileAccessMode.Read);
            var reader = new DataReader(filebuffer.GetInputStreamAt(0));
            var bytes = new byte[filebuffer.Size];
            await reader.LoadAsync((uint)filebuffer.Size);
            reader.ReadBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var fullFilePath = Path.Combine(RemoteRootPath, FileEntry.FilePath).Replace("/", @"\");
            var remoteFile = await StorageFile.GetFileFromPathAsync(fullFilePath);

            var ret = await OpenAndConvert(remoteFile);
            var jsfunction = $"openPdfAsBase64('{ret}')";

            var obj = await PdfViewerControl.InvokeScriptAsync("eval", new[] { jsfunction });

            var dialog = new MessageDialog(obj);
            await dialog.ShowAsync();
        }

        private void ElementView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ElementModel element)
            {
                _valueChanged = true;
                //AmountTextBox.Text = element.Amount.ToString();
                //AmountTextBox.Tag = element.Count;
            }
        }

        private void AmountTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (double.TryParse(args.NewText, out double amount) && double.TryParse(sender.Tag?.ToString(), out double count))
            {
                if (amount > count)
                {
                    args.Cancel = true;
                    return;
                }
            }

            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = new string(textBox.Text.Where(char.IsDigit).ToArray());
            }
        }

        private async void ButtonIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(AmountTextBox.Text, out double amount) && double.TryParse(AmountTextBox.Tag.ToString(), out double count))
            {
                if (count > amount)
                {
                    ElementCollection.SelectedElement.Amount++;
                    await Task.Delay(500);
                }
            }
        }

        private async void ButtonDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(AmountTextBox.Text, out double amount) && double.TryParse(AmountTextBox.Tag.ToString(), out double _))
            {
                if (amount > 0)
                {
                    ElementCollection.SelectedElement.Amount--;
                    await Task.Delay(500);
                }
            }
        }

        private void AmountTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (double.TryParse(AmountTextBox.Text, out double amount) && double.TryParse(AmountTextBox.Tag.ToString(), out double count))
                {
                    if (amount > count)
                    {
                        return;
                    }

                    if (amount != ElementCollection.SelectedElement.Amount)
                    {
                        ElementCollection.SelectedElement.Amount = amount;
                        ElementAmountChanged(ElementCollection.SelectedElement);
                    }
                }
            }
        }

        private void ElementAmountChanged(ElementModel element)
        {

        }

    }
}
