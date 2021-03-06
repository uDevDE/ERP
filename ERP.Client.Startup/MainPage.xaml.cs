﻿using ERP.Client.Dialogs;
using ERP.Client.Mapper;
using ERP.Client.Model;
using System;
using System.Linq;
using System.Security.Principal;
using System.Collections.ObjectModel;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ERP.Client.Startup.Extensions;
using System.Collections.Generic;
using ERP.Contracts.Domain.Core.Enums;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using ERP.Client.Core.Enums;
using ERP.Client.ViewModel;
using ERP.Client.Startup.View;
using Newtonsoft.Json;
using AutoMapper.Mappers;
using Windows.ApplicationModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ERP.Client.Startup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public AdministrationViewModel AdministrationViewModel { get; set; }

        private string CurrentPageKey;
        private TabViewCollectionModel CurrentTabView;

        public LocalClient Client { get; set; }

        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>()
        {
            { "Administration", typeof(AdministrationPage) },
            { "Configuration", typeof(ConfigurationPage) },
            { "ProjectViewer", typeof(ProjectViewerPage) },
            { "Settings", typeof(SettingsPage) },
            { "ElementView", typeof(ElementViewerPage) }
        };

        public MainPage()
        {
            this.InitializeComponent();

            AutoMapperConfiguration.Configure();

            Employees = new ObservableCollection<EmployeeModel>();

            Client = new LocalClient();
            AdministrationViewModel = new AdministrationViewModel();
        }

        private static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private void EnableLoadingControl(string message)
        {
            LoadingText.Text = message;
            LoadingControl.IsLoading = true;
        }

        private void DisableLoadingControl()
        {
            LoadingControl.IsLoading = false;
            LoadingText.Text = string.Empty;
        }

        private async void UpdateLoadingControl(string message)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LoadingText.Text = message;
            });
        }

        private void SetCurrentEmployee(EmployeeModel employee)
        {
            LocalClient.Employee = employee;
            AdministrationViewModel.IsAdministrator = employee.IsAdministrator;
        }

        private void SetCurrentDivision(DivisionModel division) => LocalClient.Division = division;

        private async void Navigator_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = args.RecommendedNavigationTransitionInfo
            };

            var navigationTransitionInfo = new SlideNavigationTransitionInfo
            {
                Effect = SlideNavigationTransitionEffect.FromLeft
            };

            if (args.IsSettingsSelected)
            {
                NavigateToSettingsPage(LocalClient.Employee, navigationTransitionInfo);
                return;
            }

            if (args.SelectedItem is NavigationViewItem navigationViewItem && navigationViewItem.Tag is string tag)
            {
                if (_pages.TryGetValue(tag, out Type type))
                {
                    /*if (tag == "ProjectViewer" && CurrentTabView != null)
                    {
                        parameter = CurrentTabView;
                    }

                    if (ContentFrame.Navigate(type, parameter, navigationTransitionInfo))
                    {
                        CurrentPageKey = tag;

                        if (tag == "ProjectViewer" && CurrentTabView == null)
                        {
                            CurrentTabView = (ContentFrame.Content as ProjectViewerPage).TabViewCollection;
                        }
                    }*/

                    if (tag == "ProjectViewer")
                    {
                        ContentFrame.Navigate(type, LocalClient.Employee, navigationTransitionInfo);
                    }
                    else if (tag == "Administration")
                    {
                        ContentFrame.Navigate(type, null, navigationTransitionInfo);
                    }
                    else if (tag == "Configuration")
                    {
                        ContentFrame.Navigate(type, null, navigationTransitionInfo);
                    }
                    else if (tag == "ElementView")
                    {             
                        if (LocalClient.Employee == null)
                        {
                            var infoDialog = new InfoDialog("Du musst Dich anmelden, um Einsicht in den aktuellen Fertigungsstatus zu bekommen!");
                            await infoDialog.ShowAsync();
                            return;
                        }
                        ContentFrame.Navigate(type, LocalClient.Employee, navigationTransitionInfo );
                    }
                }
            }

            //ContentFrame.Navigate(typeof(AdministrationPage), null, navigationTransitionInfo);
        }

        private void NavigateToSettingsPage(EmployeeModel employee, SlideNavigationTransitionInfo navigationTransitionInfo)
        {
            if (employee != null)
            {
                ContentFrame.Navigate(typeof(SettingsPage), new object[] { employee }, navigationTransitionInfo);
                //NavigationHeader.Text = $"Einstellungen von {employee.Alias}";
            }
        }

        private void PersonEmployee_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                //if (TeachingTipEmployee.IsOpen)
                //{
                    //TeachingTipEmployee.IsOpen = false;
                //}
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(1200, 600));
            EnableLoadingControl("Verbindung wird aufgebaut...");

            var clientDeviceInformation = new EasClientDeviceInformation();
            var deviceId = clientDeviceInformation.Id;
            var hostNames = NetworkInformation.GetHostNames();
            var hostname = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? null;
            var username = WindowsIdentity.GetCurrent().Name;

            if (string.IsNullOrEmpty(hostname))
            {
                var dialog = new InfoDialog("Der Hostname konnte nicht aufgelöst werden.", "Fehler", InfoDialogType.Error);
                await dialog.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(username))
            {
                var dialog = new InfoDialog("Der Username konnte nicht aufgelöst werden.", "Fehler", InfoDialogType.Error);
                await dialog.ShowAsync();
                return;
            }

            if (deviceId == null || deviceId == Guid.Empty)
            {
                var dialog = new InfoDialog("Der Geräte Identifikator konnte nicht aufgelöst werden.", "Fehler", InfoDialogType.Error);
                await dialog.ShowAsync();
                return;
            }

            await Proxy.InitializeAsync();
            Proxy.ServiceCallback.AuthorizedEvent += ServiceCallback_AuthorizedEvent;
            Proxy.ServiceCallback.ServiceMessageEvent += ServiceCallback_ServiceMessageEvent;
            Proxy.ServiceCallback.AuthorizedFailedEvent += ServiceCallback_AuthorizedFailedEvent;
            Proxy.ServiceCallback.EmployeeUpdatedEvent += ServiceCallback_EmployeeUpdatedEvent;
            bool connect = Proxy.Connect(deviceId, hostname, username);
            if (!connect)
            {
                var dialog = new InfoDialog("Der Service steht momentan nicht zur Verfügung", "Anmeldung fehlgeschlagen", InfoDialogType.Error);
                await dialog.ShowAsync();
            }

            var tempFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
            var d = new MessageDialog(tempFolder.Path);
            await d.ShowAsync();

            Proxy.DeviceId = deviceId;
        }

        private async void ServiceCallback_EmployeeUpdatedEvent(EmployeeModel employee)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SetCurrentEmployee(employee);
                Employees.Where(x => x.EmployeeId == employee.EmployeeId).ToList().ForEach(item => item = employee);
            });
        }

        private async void ServiceCallback_AuthorizedFailedEvent(AuthorisationType authorisationType)
        {
            var d = new MessageDialog("FAILED");
            await d.ShowAsync();
            switch (authorisationType)
            {
                case AuthorisationType.AuthorizeBlocked:
                    UpdateLoadingControl("Dieses Gerät wurde blockiert");
                    break;
                case AuthorisationType.AuthorizeWaitingForRelease:
                    UpdateLoadingControl("Es wird auf Freigabe dieses Gerät's gewartet");
                    break;
                case AuthorisationType.AuthorizeFailed:
                    UpdateLoadingControl("Eine Anmeldung war nicht möglich");
                    break;
                default: 
                    UpdateLoadingControl("Unzureichende Berechtigung");
                    break;
            }
        }

        private async void ServiceCallback_ServiceMessageEvent(string message)
        {
            await Dispatcher.RunTaskAsync(async () =>
            {
                var dialog = new InfoDialog(message);
                await dialog.ShowAsync();
            });
        }

        private async void ServiceCallback_AuthorizedEvent(DeviceModel device)
        {
            await Dispatcher.RunTaskAsync(async () =>
            {
                var d = new MessageDialog("SUCCESS");
                await d.ShowAsync();

                var list = await Proxy.GetAllEmployeesByDevice();
                foreach (var item in list)
                {
                    Employees.Add(item);
                }
            });

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                this.IsEnabled = true;
                DisableLoadingControl();

                if (device.Employee != null)
                {
                    NotificationControl.Show($"Willkommen, {device.Employee.Alias}", 3500);
                    SetCurrentEmployee(device.Employee);
                    Proxy.Device = device;
                }

                if (device.Division != null)
                {
                    SetCurrentDivision(device.Division);
                }
            });
        }

        private void Navigator_Loaded(object sender, RoutedEventArgs e)
        {
            if (Navigator.SettingsItem is NavigationViewItem settings)
            {
                settings.Content = "Einstellungen";
            }   
        }

        private async void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ImportEmployeeDialog();
            await dialog.ShowAsync();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            /*if (ListBoxEmployee.SelectedItem is EmployeeModel model)
            {
                var dialog = new PasswordDialog();
                var dialogResult = await dialog.ShowAsync();
                if (dialogResult == ContentDialogResult.Primary)
                {
                    var password = dialog.Password;
                    var employee = await Proxy.SwitchEmployee(model.EmployeeId, password);
                    if (employee != null)
                    {
                        SetCurrentEmployee(employee);
                    }
                    else
                    {
                        var messageDialog = new MessageDialog("Das eingegebende Passwort war falsch oder der Benuzter ist ungültig", "Information");
                        dialog.CloseButtonText = "Schließen";
                        await messageDialog.ShowAsync();
                    }
                }
            }*/
        }

        private void ButtonEmloyeeSettings_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button?.Tag is string tag)
            {
                var navigationTransitionInfo = new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                };

                if (tag == "Current")
                {
                    NavigateToSettingsPage(LocalClient.Employee, navigationTransitionInfo);
                }
                /*else if (tag == "Import" && button.DataContext is EmployeeModel employee)
                {
                    var dialog = new PasswordDialog();
                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        if (dialog.Password == employee.Password)
                        {
                            NavigateToSettingsPage(employee, navigationTransitionInfo);
                        }
                    }
                }*/
            }
        }

    }
}
