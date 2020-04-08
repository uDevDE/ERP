using ERP.Client.Model;
using ERP.Contracts.Domain.Core.Enums;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace ERP.Client.Dialogs
{
    public sealed partial class DivisionDialog : ContentDialog
    {
        public DivisionModel Division { get; set; }
        public ObservableCollection<DivisionInfoModel> DivisionInfos { get; set; }
        public List<ProcessTemplateModel> ProcessTemplates { get; private set; }

        public DivisionDialog(ObservableCollection<DivisionInfoModel> divisionInfos, List<ProcessTemplateModel> processTemplates)
        {
            this.InitializeComponent();
            Division = new DivisionModel();
            DivisionInfos = divisionInfos;
            ProcessTemplates = processTemplates;
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
