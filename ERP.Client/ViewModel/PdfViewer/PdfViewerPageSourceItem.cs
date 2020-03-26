using ERP.Client.Controls;
using System.Collections.ObjectModel;

namespace ERP.Client.ViewModel.PdfViewer
{
    public class PdfViewerPageSourceItem
    {
        public static int MaxItemsCount = 3;

        public ObservableCollection<PdfPageControl> Items { get; set; }

        public PdfViewerPageSourceItem() => Items = new ObservableCollection<PdfPageControl>();
    }
}
