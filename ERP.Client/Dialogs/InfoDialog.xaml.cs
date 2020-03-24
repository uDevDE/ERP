using ERP.Client.Core.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class InfoDialog : ContentDialog
    {
        public string DialogTitle
        {
            get { return this.Title.ToString(); }
            set { this.Title = value; }
        }

        public string Message
        {
            get { return MessageText.Text; }
            set { MessageText.Text = value; }
        }

        public InfoDialog(string message, string title = "Information", InfoDialogType infoDialogType = InfoDialogType.Info, string primaryButtonText = null, string secondaryButtonText = null)
        {
            this.InitializeComponent();
            DialogTitle = title;
            Message = message;

            if (!string.IsNullOrEmpty(primaryButtonText))
            {
                PrimaryButtonText = primaryButtonText;
            }

            if (!string.IsNullOrEmpty(secondaryButtonText))
            {
                SecondaryButtonText = secondaryButtonText;
            }

            switch (infoDialogType)
            {
                case InfoDialogType.Info:
                default:
                    {
                        InfoIcon.Visibility = Visibility.Visible;
                        ErrorIcon.Visibility = Visibility.Collapsed;
                        break;
                    }
                case InfoDialogType.Error:
                    {
                        InfoIcon.Visibility = Visibility.Collapsed;
                        ErrorIcon.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
