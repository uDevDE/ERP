using ERP.Client.Model;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace ERP.Client.Dialogs
{
    public sealed partial class DivisionDialog : ContentDialog
    {
        public DivisionModel Division { get; set; }
        public ObservableCollection<DivisionInfoModel> DivisionInfos { get; set; }

        public DivisionDialog(ObservableCollection<DivisionInfoModel> divisionInfos)
        {
            this.InitializeComponent();
            Division = new DivisionModel();
            DivisionInfos = divisionInfos;
            this.DataContext = this;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!string.IsNullOrEmpty(TextBoxName.Text) && !string.IsNullOrEmpty(TextBoxDescription.Text) && ComboBoxDivision.Items[ComboBoxDivision.SelectedIndex] is DivisionInfoModel division)
            {
                Division.Name = TextBoxName.Text;
                Division.Description = TextBoxDescription.Text;
                Division.DivisionType = division;
                Division.DivisionInfoId = division.DivisionInfoId;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

    }
}
