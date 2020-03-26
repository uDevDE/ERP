using Microsoft.Toolkit.Uwp.UI.Controls;
using ERP.Client.Dialogs;
using ERP.Client.Dialogs.Core.Enums;
using ERP.Client.Mapper;
using ERP.Client.Model;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Startup.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdministrationPage : Page
    {
        public ObservableCollection<DeviceModel> Devices { get; private set; }
        public ObservableCollection<DivisionModel> Divisions { get; set; }
        public ObservableCollection<DivisionInfoModel> DivisionInfos { get; set; }
        public ObservableCollection<EmployeeModel> Employees { get; set; }

        private DivisionModel _divisionCell;
        private DeviceModel _deviceCell;
        private EmployeeModel _employeeCell;

        public AdministrationPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            DivisionInfos = new ObservableCollection<DivisionInfoModel>();
            Devices = new ObservableCollection<DeviceModel>();
            Divisions = new ObservableCollection<DivisionModel>();
            Employees = new ObservableCollection<EmployeeModel>();   
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingControl.IsLoading = true;
            await LoadDevices();
            await LoadEmployees();
            await LoadDivisionInfos();
            await LoadDivisions();
            LoadingControl.IsLoading = false;
        }

        private async Task LoadDevices()
        {
            var list = await Proxy.GetAllDevices();
            foreach (var item in list)
            {
                Devices.Add(item);
            }
        }

        private async Task LoadEmployees()
        {
            var list = await Proxy.GetAllEmployees();
            foreach (var item in list)
            {
                Employees.Add(item);
            }
        }

        private async Task LoadDivisionInfos()
        {
            var list = await Proxy.GetAllDivisionInfos();
            foreach (var item in list)
            {
                DivisionInfos.Add(item);
            }
        }

        private async Task LoadDivisions()
        {
            var list = await Proxy.GetAllDivisions();
            foreach (var item in list)
            {
                Divisions.Add(item);
            }
        }

        private async void ButtonAddDivision_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DivisionDialog(DivisionInfos);
            var dialogResult = await dialog.ShowAsync();
            var division = dialog.Division;
            if (dialogResult == ContentDialogResult.Primary && division != null)
            {
                var divisionId = await Proxy.UpsertDivision(division);
                if (divisionId > 0)
                {
                    division.DivisionId = divisionId;
                    Divisions.Add(division);
                }
            }
        }

        private async void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EmployeeDialog(Devices);
            var dialogResult = await dialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                var employee = dialog.Employee;

                var result = await Proxy.UpsertEmployee(employee);
                if (result > 0)
                {
                    employee.EmployeeId = result;
                    employee.Password = Helpers.ClientHelper.Sha256(employee.Password);
                    Employees.Add(employee);
                }
            }
        }

        private void DataGridDivision_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.IsReadOnly)
            {
                if (e.Row.DataContext is DivisionModel division)
                {
                    _divisionCell = new DivisionModel(division);
                }
            }
        }

        private async void DataGridDivision_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && !e.Column.IsReadOnly)
            {
                if (e.Row.DataContext is DivisionModel division)
                {
                    if (_divisionCell != division)
                    {
                        var divisionInfo = DivisionInfos.Where(x => x.DivisionInfoId == division.DivisionInfoId).FirstOrDefault();
                        if (divisionInfo != null)
                        {
                            division.DivisionType = divisionInfo;
                            var result = await Proxy.UpsertDivision(division);
                            if (result > 0)
                            {
                                ShowNofificationMessage("Änderung wurde gespeichert");
                            }
                        }
                    }
                }
            }
        }


        private void DataGridDevice_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.IsReadOnly)
            {
                if (e.Row.DataContext is DeviceModel device)
                {
                    _deviceCell = new DeviceModel(device);
                }
            }
        }

        private void DataGridDevice_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && !e.Column.IsReadOnly)
            {
                if (e.Row.DataContext is DeviceModel device)
                {
                    if (_deviceCell != device)
                    {
                        //var result = await Proxy.Upser
                    }
                }
            }
        }


        private void DataGridEmployee_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.IsReadOnly)
            {
                if (e.Row.DataContext is EmployeeModel employee)
                {
                    _employeeCell = new EmployeeModel(employee);
                }
            }
        }

        private async void DataGridEmployee_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && !e.Column.IsReadOnly)
            {
                if (e.Row.DataContext is EmployeeModel employee)
                {
                    var device = Devices.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault();
                    if (device != null)
                    {
                        if (_employeeCell != employee)
                        {
                            employee.Device = device;
                            var result = await Proxy.UpsertEmployee(employee);
                            if (result > 0)
                            {
                                ShowNofificationMessage("Änderung wurde gespeichert");
                            }
                        }
                    }
                }
            }
        }

        private void ShowNofificationMessage(string message, bool error = false)
        {
            if (error)
            {
                NotificationTextSuccess.Visibility = Visibility.Collapsed;
                NotificationTextFailed.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationTextFailed.Visibility = Visibility.Collapsed;
                NotificationTextSuccess.Visibility = Visibility.Visible;
            }

            NotificationText.Text = message;
            NotificationControl.Show(3500);
        }

        private async void MenuItemDeleteDivision_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridDivision.SelectedItem is DivisionModel division)
            {
                var dialog = new YesNoDialog("Abteilung löschen?", $"Soll die Abteilung '{division.Name}' wirklich gelöscht werden?");
                await dialog.ShowAsync();
                if (dialog.Result == YesNoDialogType.Yes)
                {
                    var result = await Proxy.DeleteDivision(division.DivisionId);
                    if (result)
                    {
                        ShowNofificationMessage($"{division.Name} wurde gelöscht");
                        Divisions.Remove(division);
                    }
                    else
                    {
                        ShowNofificationMessage($"{division.Name} konnte nicht gelöscht werden", true);
                    }
                }
            }
        }
    }
}
