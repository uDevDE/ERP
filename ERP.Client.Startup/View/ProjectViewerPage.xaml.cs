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
using Windows.UI.Popups;

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
        public EmployeeViewModel EmployeeCollection { get; private set; }

        public ProjectViewerPage()
        {
            this.InitializeComponent();
            LoadingControl.IsLoading = true;
            _projects = new Dictionary<string, FolderModel>();
            TabViewCollection = new TabViewCollectionModel();
            EmployeeCollection = new EmployeeViewModel();

            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is EmployeeModel employee)
            {
                //EmployeeCollection.Employees.Add(employee);
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
                var textBlock = new TextBlock() { Text = jobHeader, FontSize = 18 };
                var textBlock2 = new TextBlock() { Text = name, FontSize = 16, FontStyle = Windows.UI.Text.FontStyle.Italic };
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
            var employees = await Proxy.GetAllEmployeesByDevice();
            foreach (var employee in employees)
            {
                if (employee.EmployeeId == Proxy.Device?.EmployeeId && employee.IsLoggedIn && employee.KeepConnected)
                {
                    employee.IsSelected = true;
                }
                EmployeeCollection.Employees.Add(employee);
            }

            _projects = await Proxy.GetAllProjects();
            //TabViewControl.Items.Add(CreateNewTab());
            TabViewCollection.Tabs.Add(CreateNewTab());

            LoadingControl.IsLoading = false;
        }

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item is EmployeeModel employee)
            {
                foreach (var employeeItem in EmployeeCollection.Employees)
                {
                    employeeItem.IsSelected = false;
                }

                employee.IsSelected = true;
                //var dialog = new MessageDialog(employee.Password);
                //await dialog.ShowAsync();
                if (employee.IsLoggedIn && employee.KeepConnected)
                {
                    var newEmployee = await Proxy.SwitchEmployee(employee.EmployeeId, employee.Password, employee.KeepConnected);
                    if (newEmployee != null)
                    {
                        //SetCurrentEmployee(employee);
                        LocalClient.Employee = newEmployee;
                        employee.KeepConnected = newEmployee.KeepConnected;
                        employee.IsLoggedIn = true;
                        NotificationControl.Show($"Angemeldet als {employee.Fullname}", 2000);
                    }
                    else
                    {
                        var messageDialog = new InfoDialog("Das eingegebende Passwort war falsch oder der Benuzter ist ungültig");
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        private async void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ImportEmployeeDialog();
            await dialog.ShowAsync();
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is EmployeeModel employee)
            {
                var dialog = new PasswordDialog(employee.Fullname, employee.KeepConnected);
                var dialogResult = await dialog.ShowAsync();
                if (dialogResult == ContentDialogResult.Primary)
                {
                    var newEmployee = await Proxy.SwitchEmployee(employee.EmployeeId, dialog.Password, dialog.KeepConnected);
                    if (newEmployee != null)
                    {
                        //SetCurrentEmployee(employee);
                        LocalClient.Employee = newEmployee;
                        employee.KeepConnected = newEmployee.KeepConnected;
                        employee.IsLoggedIn = true;
                    }
                    else
                    {
                        var messageDialog = new InfoDialog("Das eingegebende Passwort war falsch oder der Benuzter ist ungültig");
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        private async void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is EmployeeModel employee)
            {
                var result = await Proxy.Logout(employee.EmployeeId);
                if (result)
                {
                    employee.IsLoggedIn = false;
                    employee.KeepConnected = false;
                }
                else
                {
                    var dialog = new InfoDialog("Die Abmeldung konnte nicht abgeschlossen werden, da ein interner Fehler aufgetreten ist!");
                    await dialog.ShowAsync();
                }
            }
        }


    }
}
