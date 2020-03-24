using ERP.Client.Model;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class DivisionInfoDialog : ContentDialog
    {
        public DivisionInfoModel DivisionInfo { get; set; }
        public List<DivisionType> DivisionTypes { get; set; }

        private SolidColorBrush _colorSuccess;
        private SolidColorBrush _colorFailed;

        public DivisionInfoDialog()
        {
            this.InitializeComponent();

            _colorFailed = Helpers.ClientHelper.GetSolidColorBrush("#ff9e0f05");
            _colorSuccess = new SolidColorBrush() { Color = (Windows.UI.Color)this.Resources["SystemAccentColor"] };
            DivisionTypes = new List<DivisionType>();

            foreach (DivisionType divisionType in (DivisionType[])Enum.GetValues(typeof(DivisionType)))
            {
                DivisionTypes.Add(divisionType);
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText.Visibility = Visibility.Collapsed;
            TextBoxName.BorderBrush = _colorSuccess;
            TextBoxMachinePath.BorderBrush = _colorSuccess;

            var name = TextBoxName.Text;
            var path = TextBoxMachinePath.Text;
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                ErrorText.Text = "Der Name enthält keinen gültigen Wert";
                ErrorText.Visibility = Visibility.Visible;
                TextBoxName.BorderBrush = _colorFailed;
                args.Cancel = true;
                return;
            }

            if (ComboBoxType.SelectedItem is DivisionType divisionType)
            {
                if (divisionType == DivisionType.Machine)
                {
                    if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                    {
                        ErrorText.Text = "Der Maschinen Pfad muss auf einen gültigen Pfad verweisen";
                        ErrorText.Visibility = Visibility.Visible;
                        TextBoxMachinePath.BorderBrush = _colorFailed;
                        args.Cancel = true;
                        return;
                    }
                }

                DivisionInfo = new DivisionInfoModel()
                {
                    Name = name,
                    Description = TextBoxDescription.Text,
                    DivisionType = divisionType,
                    MachinePath = path
                };
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxType.SelectedItem is DivisionType divisionType)
            {
                if (divisionType == DivisionType.Machine)
                {
                    TextBoxMachinePath.Visibility = Visibility;
                }
                else
                {
                    TextBoxMachinePath.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
