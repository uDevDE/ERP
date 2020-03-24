using ERP.Client.Dialogs.Core.Enums;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class YesNoDialog : ContentDialog
    {
        public YesNoDialogType Result { get; private set; }

        public YesNoDialog(string title, string message)
        {
            this.InitializeComponent();

            Title = title;
            MessageText.Text = message;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => Result = YesNoDialogType.Yes;

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => Result = YesNoDialogType.No;

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => Result = YesNoDialogType.Abort;
    }
}
