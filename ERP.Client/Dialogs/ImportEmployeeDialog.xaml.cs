using ERP.Client.Mapper;
using ERP.Client.Model;
using ERP.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class ImportEmployeeDialog : ContentDialog
    {
        public ObservableCollection<EmployeeModel> Employees { get; set; }

        public ImportEmployeeDialog()
        {
            this.InitializeComponent();
            Employees = new ObservableCollection<EmployeeModel>();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var list = await Proxy.GetAllEmployees();
            if (list != null)
            {
                foreach (var item in list)
                {
                    Employees.Add(item);
                }
            }
        }
    }
}
