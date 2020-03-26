using ERP.Client.Core.Enums;
using ERP.Client.Dialogs;
using ERP.Client.Model;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Startup.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public EmployeeModel Employee { get; set; }
        public bool ColorChanged { get; private set; }
        private string _color;

        private SolidColorBrush _colorSuccess;
        private SolidColorBrush _colorFailed;

        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            _colorFailed = Helpers.ClientHelper.GetSolidColorBrush("#ff9e0f05");
            _colorSuccess = new SolidColorBrush() { Color = (Windows.UI.Color)this.Resources["SystemAccentColor"] };

            if (!string.IsNullOrEmpty(Employee?.Color))
            {
                try
                {
                    var brush = Helpers.ClientHelper.GetSolidColorBrush(Employee.Color);
                    ColorPickerEmployee.Color = brush.Color;
                }
                catch (Exception) { }
            }
        }

        private void ColorPickerEmployee_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            if (!ColorChanged)
            {
                ColorChanged = true;
                ButtonSaveColor.IsEnabled = ColorChanged;
            }

            _color = args.NewColor.ToString();
            PersonPicturePreview.Foreground = new SolidColorBrush(args.NewColor);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            object[] passedParameter = e.Parameter as object[];

            if (passedParameter[0] is EmployeeModel employee)
            {
                Employee = employee;
            }
            base.OnNavigatedTo(e);
        }

        private async void ButtonSaveColor_Click(object sender, RoutedEventArgs e)
        {
            string message;
            string title;
            InfoDialogType dialogType = InfoDialogType.Info;
            if (Employee != null && !string.IsNullOrEmpty(_color))
            {
                var result = await Proxy.ChangeEmployeeForegroundColor(Employee.EmployeeId, _color);           
                if (result)
                {
                    Proxy.UpdateEmployee(Employee);
                    message = "Die Schriftfarbe Deiner Anmeldeinformationsbox wurde geändert";
                    title = "Erfolg";
                }
                else
                {
                    message = "Die Schriftfarbe konnte nicht gespeichert werden";
                    title = "Fehler";
                    dialogType = InfoDialogType.Error;
                }
            }
            else
            {
                message = "Ein Fehler ist aufgetreten";
                title = "Fehler";
                dialogType = InfoDialogType.Error;
            }
            var dialog = new InfoDialog(message, title, dialogType);
            await dialog.ShowAsync();
        }

        private async void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidatePassword())
                return;

            string message;
            string title;
            InfoDialogType dialogType = InfoDialogType.Info;
            if (Employee != null)
            {
                var password = PasswordBoxPassword.Password;
                var newPassword = PasswordBoxNewPassword.Password;

                var result = await Proxy.ChangeEmployePassword(Employee.EmployeeId, password, newPassword);
                if (result)
                {
                    Proxy.UpdateEmployee(Employee);
                    message = "Dein Passwort wurde geändert";
                    title = "Erfolg";
                }
                else
                {
                    message = "Das eingegebende Passwort war falsch";
                    title = "Fehler";
                    dialogType = InfoDialogType.Error;
                }
            }
            else
            {
                message = "Ein Fehler ist aufgetreten";
                title = "Fehler";
                dialogType = InfoDialogType.Error;
            }

            var dialog = new InfoDialog(message, title, dialogType);
            await dialog.ShowAsync();
        }

        private bool ValidatePassword()
        {
            PasswordBoxPassword.BorderBrush = _colorSuccess;
            PasswordBoxPasswordRepeat.BorderBrush = _colorSuccess;
            PasswordBoxNewPassword.BorderBrush = _colorSuccess;
            ErrorText.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(PasswordBoxPassword.Password))
            {
                PasswordBoxPassword.BorderBrush = _colorFailed;
                ErrorText.Text = "Das rot markierte Passwortfeld enthält kein gültiges Passwort";
                ErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrEmpty(PasswordBoxPasswordRepeat.Password))
            {
                PasswordBoxPasswordRepeat.BorderBrush = _colorFailed;
                ErrorText.Text = "Das rot markierte Passwortfeld (Passwort wiederholen) enthält kein gültiges Passwort";
                ErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (PasswordBoxPassword.Password != PasswordBoxPasswordRepeat.Password)
            {
                PasswordBoxPassword.BorderBrush = _colorFailed;
                PasswordBoxPasswordRepeat.BorderBrush = _colorFailed;
                ErrorText.Text = "Die eingegebenden Passwörter stimmen nicht überein";
                ErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrEmpty(PasswordBoxNewPassword.Password))
            {
                PasswordBoxNewPassword.BorderBrush = _colorFailed;
                ErrorText.Text = "Das rot markierte Passwortfeld (Neues Passwort) enthält kein gültiges Passwort";
                ErrorText.Visibility = Visibility.Visible;
                return false;
            }

            return true;

        }

    }
}
