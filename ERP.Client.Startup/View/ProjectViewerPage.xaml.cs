using Microsoft.Toolkit.Uwp.UI.Controls;
using ERP.Client.Model;
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
using ERP.Client.Core.Enums;
using ERP.Client.Dialogs;
using ERP.Client.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Startup.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectViewerPage : Page
    {
        private static int index = 0;
        private Dictionary<string, FolderModel> _projects;

        public TabViewCollectionModel TabViewCollection { get; private set; }

        public ProjectViewerPage()
        {
            this.InitializeComponent();
            LoadingControl.IsLoading = true;
            _projects = new Dictionary<string, FolderModel>();
            TabViewCollection = new TabViewCollectionModel();

            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is TabViewCollectionModel tabViewCollection)
            {
                TabViewCollection = tabViewCollection;
            }

            base.OnNavigatedTo(e);
        }

        private void ButtonAddTag_Click(object sender, RoutedEventArgs e)
        {
            var tab = CreateNewTab();
            //TabViewControl.Items.Add(tab);
            TabViewCollection.Tabs.Add(tab);
            tab.IsSelected = true;
        }


        private TabViewItem CreateNewTab()
        {
            index++;
            TabViewItem newItem = new TabViewItem
            {
                Header = "Neues Projekt",
                Icon = new SymbolIcon(Symbol.Document),
                Tag = index
            };

            Frame frame = new Frame();
            frame.Navigated += Frame_Navigated;
            var result = frame.Navigate(typeof(ProjectPreviewPage), index);

            newItem.Content = frame;
            return newItem;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                if (e.Content is ProjectPreviewPage pdfViewerPreviewPage)
                {
                    pdfViewerPreviewPage.ButtonOpenProjectClicked += PdfViewerPreviewPage_ButtonOpenProjectClicked;
                    pdfViewerPreviewPage.Load(_projects);
                }
            }
        }

        private TabViewItem FindTab(int index)
        {
            foreach (var tab in TabViewCollection.Tabs)
            {
                if (tab is TabViewItem tabItem)
                {
                    if (tabItem.Tag is int idx)
                    {
                        if (index == idx)
                        {
                            return tabItem;
                        }
                    }
                }
            }
            return null;
        }

        private void UpdateTabViewItemHeader(TabViewItem tabItem, FolderModel folder, string name)
        {
            if (folder.IsJob)
            {
                var folderName = folder.Name;
                var projectIdentifier = folderName.Split('-').FirstOrDefault().Trim();
                var jobName = folderName.Split('-').LastOrDefault().Trim();
                var jobHeader = $"{projectIdentifier} - {jobName}";

                var panel = new StackPanel();
                var textBlock = new TextBlock() { Text = jobHeader };
                var textBlock2 = new TextBlock() { Text = name };
                panel.Children.Add(textBlock);
                panel.Children.Add(textBlock2);

                tabItem.Header = panel;
            }
        }

        private string GetProjectIdentifier(ProjectPreviewPage projectPreviewPage)
        {
            var fileEntry = projectPreviewPage.SelectedFileEntry;
            var plantOrder = projectPreviewPage.SelectedPlantOrder;

            if (fileEntry != null)
            {
                if (fileEntry.FileInfo != null)
                {
                    return $"{fileEntry.FileInfo.ProjectIdentifier}    {fileEntry.FileInfo.Section}";
                }

                return fileEntry.Name;
            }
            else if (plantOrder != null)
            {
                return $"{plantOrder.Number}    {plantOrder.Section}";
            }
            else
            {
                return null;
            }
        }

        private async void PdfViewerPreviewPage_ButtonOpenProjectClicked(ProjectPreviewPage projectPreviewPage, ProjectPreviewType projectPreviewType, FolderModel project)
        {
            var tabItem = FindTab(projectPreviewPage.Index);
            if (tabItem != null)
            {
                var name = GetProjectIdentifier(projectPreviewPage);
                if (name != null)
                {
                    UpdateTabViewItemHeader(tabItem, project, name);

                    var plantOrder = projectPreviewPage.SelectedPlantOrder;
                    var fileEntry = projectPreviewPage.SelectedFileEntry;

                    if (fileEntry == null && plantOrder != null)
                    {
                        fileEntry = projectPreviewPage.FindFilename(plantOrder);
                    }

                    if (tabItem.Content is Frame frame && fileEntry != null)
                    {      
                        frame.Navigate(typeof(ProjectContentPage), new object[] { projectPreviewType, project, fileEntry, plantOrder });
                    }
                    else
                    {
                        var dialog = new InfoDialog("Es wurde leider keine Datei gefunden", "Information", InfoDialogType.Error);
                        await dialog.ShowAsync();
                    }
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _projects = await Proxy.GetAllProjects();
            //TabViewControl.Items.Add(CreateNewTab());
            TabViewCollection.Tabs.Add(CreateNewTab());

            LoadingControl.IsLoading = false;
        }
    }
}
