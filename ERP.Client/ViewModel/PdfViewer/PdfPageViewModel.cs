using ERP.Client.Controls;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace ERP.Client.ViewModel.PdfViewer
{
    public class PdfPageViewModel : INotifyPropertyChanged
    {
        private PdfViewerPageSource _pages;
        private Orientation _orientation;

        public PdfPageControl CurrentPage { get; set; }
        public uint PageCount { get; private set; }
        public bool IsEmpty { get { return _pages.Items.Count == 0; } }
        public Size MaxPageSize { get; private set; }
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    RaisePropertyChanged("Orientation");
                }
            }
        }

        public PdfViewerPageSource Pages
        {
            get { return _pages; }
            set
            {
                if (_pages != value)
                {
                    _pages = value;
                    RaisePropertyChanged("Pages");
                }
            }
        }

        public void Add(PdfPageControl pageControl)
        {
            if (_pages.Add(pageControl))
            {
                if (pageControl.Page.Size.Width > MaxPageSize.Width)
                {
                    MaxPageSize = pageControl.Page.Size;
                }
                PageCount++;
            }
        }

        public PdfPageControl GetCurrentPage()
        {
            double val = double.MaxValue;
            PdfPageControl result = null;
            foreach (var item in _pages.Items)
            {
                foreach (var pageControl in item.Items)
                {
                    if (pageControl.PageLoaded && pageControl.DistanceY < val)
                    {
                        val = pageControl.DistanceY;
                        result = pageControl;
                    }
                }
            }

            CurrentPage = result;
            return result;
        }

        public PdfPageControl GetPage(uint index)
        {
            foreach (var item in _pages.Items)
            {
                foreach (var page in item.Items)
                {
                    if (page.Page.Index == (index - 1))
                    {
                        return page;
                    }
                }
            }

            return null;
        }

        public PdfPageControl GetLastPage()
        {
            return _pages.Items.Last()?.Items.Last();
        }

        public void SetPreviewMode(bool mode)
        {
            foreach (var item in _pages.Items)
            {
                foreach (var page in item.Items)
                {
                    page.SetPreviewMode(mode);
                }
            }
        }

        public PdfPageViewModel()
        {
            _pages = new PdfViewerPageSource();
            _orientation = Orientation.Vertical;
            MaxPageSize = new Size(0, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
