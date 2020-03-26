using ERP.Client.Dialogs;
using ERP.Client.Model;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Startup.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigurationPage : Page
    {
        public ObservableCollection<DivisionInfoModel> DivisionInfos { get; set; }
        public List<DivisionType> DivisionTypes { get; set; }

        public ConfigurationPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            DivisionInfos = new ObservableCollection<DivisionInfoModel>();
            DivisionTypes = new List<DivisionType>();

            foreach (DivisionType divisionType in (DivisionType[])Enum.GetValues(typeof(DivisionType)))
            {
                DivisionTypes.Add(divisionType);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var list = await Proxy.GetAllDivisionInfos();
            foreach (var item in list)
            {
                DivisionInfos.Add(item);
            }
        }

        private async void ButtonAddDivisionInfo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DivisionInfoDialog();
            var dialogResult = await dialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                var divisionInfo = dialog.DivisionInfo;
                if (divisionInfo != null)
                {
                    var divisionInfoId = await Proxy.UpsertDivisionInfo(divisionInfo);
                    if (divisionInfoId > 0)
                    {
                        divisionInfo.DivisionInfoId = divisionInfoId;
                        DivisionInfos.Add(divisionInfo);
                    }
                }
            }
        }
    }
}
