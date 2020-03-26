using ERP.Client.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace ERP.Client.ViewModel.PdfViewer
{
    public class PdfViewerPageSource
    {
        public PdfViewerPageSource() => Items = new ObservableCollection<PdfViewerPageSourceItem>();

        public ObservableCollection<PdfViewerPageSourceItem> Items { get; set; }

        public bool Add(PdfPageControl pageControl)
        {
            if (Items.Count == 0)
            {
                AddNewSourceItem(pageControl);
                return true;
            }

            var sourceItem = Items.Last();
            if (sourceItem != null)
            {
                if (sourceItem.Items.Count == PdfViewerPageSourceItem.MaxItemsCount)
                {
                    AddNewSourceItem(pageControl);
                    return true;
                }

                sourceItem.Items.Add(pageControl);
                return true;
            }

            return false;
        }

        private void AddNewSourceItem(PdfPageControl pageControl)
        {
            var item = new PdfViewerPageSourceItem();
            item.Items.Add(pageControl);

            Items.Add(item);
        }
    }
}
