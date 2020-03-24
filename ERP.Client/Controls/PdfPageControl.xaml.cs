using System;
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ERP.Client.Controls
{
    public sealed partial class PdfPageControl : UserControl
    {
        public delegate void PdfPageLoadedHandler();
        public event PdfPageLoadedHandler PdfPageLoaded;

        public delegate void PdfPageClickedHandler(PdfPageControl pdfViewerPage);
        public event PdfPageClickedHandler PdfPageClicked;

        const int WrongPassword = unchecked((int)0x8007052b); // HRESULT_FROM_WIN32(ERROR_WRONG_PASSWORD)
        const int GenericFail = unchecked((int)0x80004005);   // E_FAIL

        private PdfPage _page;
        private uint _pageIndex;

        public PdfPageControl()
        {
            this.InitializeComponent();
        }

        public uint PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (_pageIndex != value)
                {
                    _pageIndex = value;
                }
            }
        }

        public ImageSource Source
        {
            get { return PdfPageImage.Source; }
            set { PdfPageImage.Source = value; }
        }

        public Stretch Stretch
        {
            get { return PdfPageImage.Stretch; }
            set { PdfPageImage.Stretch = value; }
        }

        public PdfPage Page
        {
            get { return _page; }
            set
            {
                if (_page != value)
                {
                    _page = value;
                    _pageIndex = _page.Index;
                    RenderPage();
                }
            }
        }

        public UIElement RootUIElement
        {
            get { return PdfPageBorder; }
        }

        private async void RenderPage()
        {
            var stream = new InMemoryRandomAccessStream();
            await _page.RenderToStreamAsync(stream);
            BitmapImage src = new BitmapImage();
            PdfPageImage.Source = src;
            await src.SetSourceAsync(stream);
            PdfPageLoaded?.Invoke();
        }


    }
}
