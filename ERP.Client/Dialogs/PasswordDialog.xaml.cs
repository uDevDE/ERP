using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class PasswordDialog : ContentDialog
    {
        public PasswordDialog()
        {
            this.InitializeComponent();
        }

        public string Password { get; private set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Password = Helpers.ClientHelper.Sha256(PwBox.Password);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
