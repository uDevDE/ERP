using ERP.Client.Model;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class PasswordDialog : ContentDialog
    {
        public string Password { get; private set; }
        public bool KeepConnected { get; private set; }
        public string EmployeeName { get; private set; }

        public PasswordDialog(string employeeFullname, bool keepConnected)
        {
            this.InitializeComponent();

            EmployeeName = employeeFullname;
            KeepConnected = keepConnected;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Password = Helpers.ClientHelper.Sha256(PwBox.Password);
            if (KeepConnectedCheckBox.IsChecked.HasValue)
            {
                KeepConnected = KeepConnectedCheckBox.IsChecked.Value;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
