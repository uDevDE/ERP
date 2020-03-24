using ERP.Client.Model;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Collections.ObjectModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class EmployeeDialog : ContentDialog
    {
        public EmployeeModel Employee { get; set; }
        public ObservableCollection<DeviceModel> Devices { get; private set; }

        private SolidColorBrush _colorSuccess;
        private SolidColorBrush _colorFailed;

        public EmployeeDialog(ObservableCollection<DeviceModel> devices)
        {
            this.InitializeComponent();
            _colorFailed = Helpers.ClientHelper.GetSolidColorBrush("#ff9e0f05");
            _colorSuccess = new SolidColorBrush() { Color = (Windows.UI.Color)this.Resources["SystemAccentColor"] };
            Devices = new ObservableCollection<DeviceModel>();
            foreach (var item in devices)
                Devices.Add(item);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            TextErrorMessage.Visibility = Visibility.Collapsed;
            TextPasswordErrorMessage.Visibility = Visibility.Collapsed;
            TextBoxNumber.BorderBrush = _colorSuccess;
            TextBoxFirstname.BorderBrush = _colorSuccess;
            TextBoxLastname.BorderBrush = _colorSuccess;
            PasswordBoxPassword.BorderBrush = _colorSuccess;
            PasswordBoxPasswordRepeat.BorderBrush = _colorSuccess;

            var firstname = TextBoxFirstname.Text.Trim();
            var lastname = TextBoxLastname.Text.Trim();
            var alias = TextBoxAlias.Text.Trim();
            var password = PasswordBoxPassword.Password;
            var passwordRepeat = PasswordBoxPasswordRepeat.Password;
            string description = TextBoxDescription.Text;
            int number;
            var device = ComboBoxDevice.SelectedItem as DeviceModel;

            if (!int.TryParse(TextBoxNumber.Text, out number) || number <= 0)
            {
                TextBoxNumber.BorderBrush = Helpers.ClientHelper.GetSolidColorBrush("#ff9e0f05");
                TextErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(firstname) || string.IsNullOrWhiteSpace(firstname))
            {
                TextBoxFirstname.BorderBrush = _colorFailed;
                TextErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(lastname) || string.IsNullOrWhiteSpace(lastname))
            {
                TextBoxLastname.BorderBrush = _colorFailed;
                TextErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                PasswordBoxPassword.BorderBrush = _colorFailed;
                TextErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(passwordRepeat) || string.IsNullOrWhiteSpace(passwordRepeat))
            {
                PasswordBoxPasswordRepeat.BorderBrush = _colorFailed;
                TextErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(alias) || string.IsNullOrWhiteSpace(alias))
            {
                alias = string.Format("{0:s} {1:s}", firstname, lastname);
            }

            if (password != passwordRepeat)
            {
                TextPasswordErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
                return;
            }

            Employee = new EmployeeModel()
            {
                Number = number,
                Firstname = firstname,
                Lastname = lastname,
                Alias = alias,
                Description = description,
                Password = password,
                Device = device,
                DeviceId = device.DeviceId
            };
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private void TextBoxNumber_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = System.Text.RegularExpressions.Regex.IsMatch(args.NewText, "[^0-9]+");
        }

    }
}
