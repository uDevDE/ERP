using ERP.Client.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Client.ViewModel
{
    public class PdfViewerViewModel : INotifyPropertyChanged
    {
        private PdfPageControl _currentPage;
        private ObservableCollection<PdfPageControl> _pages { get; set; }
        public PdfPageControl GetLastPage() => _pages.LastOrDefault();
        public PdfPageControl GetPage(uint index) => _pages.FirstOrDefault(p => p.Page.Index == (index - 1));
        public PdfPageControl GetFirstPage() => _pages.FirstOrDefault();
        public PdfPageControl CurrentPage
        {
            get
            {
                return _currentPage == null ? _pages?.FirstOrDefault() : _currentPage;
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
            foreach (var page in _pages)
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

        public ObservableCollection<PdfPageControl> Pages
        {
            get { return _pages; }
            set
            {
                if (_pages != value)
                {
                    _pages = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PdfViewerViewModel() => _pages = new ObservableCollection<PdfPageControl>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
