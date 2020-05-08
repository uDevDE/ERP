using ERP.Client.Core.Enums;
using ERP.Client.Dialogs;
using ERP.Client.Lib;
using ERP.Client.Model;
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
        public bool PageLoaded { get; private set; }

        public ProjectPreviewType ProjectPreviewType { get; private set; }
        public FolderModel Folder { get; private set; }
        public FileEntryModel FileEntry { get; private set; }
        public PlantOrderModel PlantOrder { get; private set; }

        public MaterialRequirementsViewModel MaterialRequirementsViewModel { get; set; }
        public ObservableCollection<ProcessQRImageModel> ProcessQRImages { get; private set; }
        public PdfPageViewModel PageViewModel { get; set; }

        public ProjectContentPage()
        {
            this.InitializeComponent();

            MaterialRequirementsViewModel = new MaterialRequirementsViewModel();
            ProcessQRImages = new ObservableCollection<ProcessQRImageModel>();
            LoadingControl.IsLoading = true;
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
                var dialog = new MessageDialog(PlantOrder.Number);
                await dialog.ShowAsync();
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

            //QRImage.Source = await QRCodeImageGenerator.Generate(PlantOrder.ProcessId, 12);
            var qrdialog = new MessageDialog(PlantOrder.ProcessId);
            await qrdialog.ShowAsync();

            var materialRequirements = await Proxy.GetMaterialRequirements(new string[] { "1", "2" });
            if (materialRequirements != null)
            {
                MaterialRequirementsViewModel.Load(materialRequirements);
                DataGridMaterialRequirements.ItemsSource = MaterialRequirementsViewModel.GroupData().View;
            }

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


    }
}
