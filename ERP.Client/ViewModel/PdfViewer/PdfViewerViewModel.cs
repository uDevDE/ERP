using ERP.Client.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace ERP.Client.ViewModel.PdfViewer
{
    public class PdfViewerViewModel
    {
        private PdfPageControl _currentPage;
        public PdfPageControl GetLastPage() => Pages.LastOrDefault();
        public PdfPageControl GetPage(uint index) => Pages.FirstOrDefault(p => p.Page.Index == (index - 1));
        public PdfPageControl GetFirstPage() => Pages.FirstOrDefault();
        public PdfPageControl CurrentPage
        {
            get
            {
                return _currentPage == null ? Pages?.FirstOrDefault() : _currentPage;
            }
            set
            {
                _currentPage = value;
            }
        }

        public PdfPageControl GetCurrentPage()
        {
            double val = double.MaxValue;
            PdfPageControl result = null;
            foreach (var page in Pages)
            {
                if (page.PageLoaded && page.DistanceY < val)
                {
                    val = page.DistanceY;
                    result = page;
                }
            }

            CurrentPage = result;
            return result;
        }

        public ObservableCollection<PdfPageControl> Pages { get; set; }

        public PdfViewerViewModel() => Pages = new ObservableCollection<PdfPageControl>();
    }
}
