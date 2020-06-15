using ERP.Client.Dialogs.Core.Enums;
using ERP.Client.Model;
using ERP.Contracts.Domain;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class ProfileDialog : ContentDialog
    {
        private int _id;
        private SolidColorBrush _colorSuccess;
        private SolidColorBrush _colorFailed;

        public ProfileDTO Result { get; private set; }
        public ProfileDialogMode Mode { get; private set;}

        public ProfileDialog()
        {
            this.InitializeComponent();
            Mode = ProfileDialogMode.Create;
            Init();
        }

        public ProfileDialog(ElementModel element)
        {
            this.InitializeComponent();
            PrimaryButtonText = "Aktualisieren";
            Mode = ProfileDialogMode.Edit;
            _id = element.Id;
            Init();

            TextBoxProfileNumber.IsEnabled = false;
            TextBoxLength.IsEnabled = false;
            TextBoxSurface.IsEnabled = false;

            TextBoxProfileNumber.Text = element.Position;
            TextBoxCount.Text = element.Count.ToString();
            TextBoxAmount.Text = element.Amount.ToString();
            TextBoxLength.Text = element.Length;
            TextBoxDescription.Text = element.Description;
            TextBoxSurface.Text = element.Surface;

            List<string> items = ConboBoxContraction.Items.Cast<ComboBoxItem>().Select(item => item.Content.ToString()).ToList();
            ConboBoxContraction.SelectedIndex = items.FindIndex(x => x.Equals(element.Contraction));

        }

        private void Init()
        {
            _colorFailed = Helpers.ClientHelper.GetSolidColorBrush("#ff9e0f05");
            _colorSuccess = new SolidColorBrush() { Color = (Windows.UI.Color)this.Resources["SystemAccentColor"] };
        }

        private bool CheckDataInput()
        {
            double count;
            double amount;
            bool result = true;
            ErrorText.Visibility = Visibility.Collapsed;
            TextBoxProfileNumber.BorderBrush = _colorSuccess;
            TextBoxCount.BorderBrush = _colorSuccess;
            TextBoxAmount.BorderBrush = _colorSuccess;
            TextBoxLength.BorderBrush = _colorSuccess;

            if (string.IsNullOrWhiteSpace(TextBoxProfileNumber.Text))
            {
                ErrorText.Visibility = Visibility.Visible;
                TextBoxProfileNumber.BorderBrush = _colorFailed;
                result =  false;
            }

            if (!double.TryParse(TextBoxCount.Text, out count))
            {
                ErrorText.Visibility = Visibility.Visible;
                TextBoxCount.BorderBrush = _colorFailed;
                result = false;
            }

            if (!double.TryParse(TextBoxAmount.Text, out amount))
            {
                ErrorText.Visibility = Visibility.Visible;
                TextBoxAmount.BorderBrush = _colorFailed;
                result = false;
            }

            if (!double.TryParse(TextBoxLength.Text, out double length))
            {
                ErrorText.Visibility = Visibility.Visible;
                TextBoxLength.BorderBrush = _colorFailed;
                result = false;
            }

            if (amount > count)
            {
                ErrorText.Visibility = Visibility.Visible;
                TextBoxCount.BorderBrush = _colorFailed;
                TextBoxAmount.BorderBrush = _colorFailed;
                result = false;
            }

            return result;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!CheckDataInput())
            {
                args.Cancel = true;
                return;
            }

            Result = new ProfileDTO() 
            { 
                ProfileId = _id,
                ProfileNumber = TextBoxProfileNumber.Text,
                Description = TextBoxDescription.Text,
                Surface = TextBoxSurface.Text,
                Length = TextBoxLength.Text,
                Contraction = (ConboBoxContraction.SelectedItem as ComboBoxItem).Content.ToString()
            };

            Result.Count = double.Parse(TextBoxCount.Text);
            Result.Amount = double.Parse(TextBoxAmount.Text);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void TextBoxAmount_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args) => args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
    }
}
