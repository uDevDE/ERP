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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectViewerPage : Page
    {
        private static int index = 0;
        private Dictionary<string, FolderModel> _projects;

        public ProjectViewerPage()
        {
            this.InitializeComponent();
            LoadingControl.IsLoading = true;
            _projects = new Dictionary<string, FolderModel>();
        }

        private void ButtonAddTag_Click(object sender, RoutedEventArgs e)
        {
            var tab = CreateNewTab();
            TabViewControl.Items.Add(tab);
            tab.IsSelected = true;
        }


        private TabViewItem CreateNewTab()
        {
            index++;
            TabViewItem newItem = new TabViewItem
            {
                Header = $"Dokument {index}",
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
                    pdfViewerPreviewPage.Load(_projects);
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _projects = await Proxy.GetAllProjects();
            //_ = _projects;
            TabViewControl.Items.Add(CreateNewTab());

            LoadingControl.IsLoading = false;
        }
    }
}
