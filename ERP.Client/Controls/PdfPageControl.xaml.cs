using System;
using System.Threading.Tasks;
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

        public double ImageRotation { get; private set; }

        public PdfPage Page { get; private set; }
        public double DistanceX { get; private set; }
        public double DistanceY { get; private set; }
        public bool PageLoaded { get; private set; }
        public bool IsFirstPage => Page?.Index == 0 ? true : false;
        public bool PreviewMode { get; private set; }

        public PdfPageControl()
        {
            this.InitializeComponent();

            this.DataContext = this;
            SetPreviewMode(PreviewMode);
        }

        public ImageSource Source
        {
            get { return PdfImage.Source; }
            set { PdfImage.Source = value; }
        }

        public Stretch Stretch
        {
            get { return PdfImage.Stretch; }
            set { PdfImage.Stretch = value; }
        }

        public UIElement RootUIElement
        {
            get { return PdfViewBox; }
        }

        public void SetPreviewMode(bool mode)
        {
            if (mode)
            {
                PdfImage.Width = 460;
                PdfImage.Height = 580;
                PdfImage.Stretch = Stretch.Fill;
            }
            else
            {
                PdfImage.Height = double.NaN;
                PdfImage.Width = double.NaN;
                PdfImage.Stretch = Stretch.None;
            }

            PreviewMode = mode;
        }

        public async Task Load(PdfPage page)
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
                Page = page;
                var bitmap = new BitmapImage();
                await page.RenderToStreamAsync(stream);
                await bitmap.SetSourceAsync(stream);
                PdfImage.Source = bitmap;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PageLoaded = true;
        }

        private void UserControl_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
        {
            DistanceX = args.BringIntoViewDistanceX;
            DistanceY = args.BringIntoViewDistanceY;
        }

        public void RotateRight()
        {
            var newValue = ImageRotation + 90;
            if (newValue > 360)
            {
                newValue = 90;
            }
            ImageRotation = newValue;

            var rotateTransform = new RotateTransform()
            {
                CenterX = PdfImage.ActualWidth / 2,
                CenterY = PdfImage.ActualHeight / 2,
                Angle = ImageRotation
            };
            PdfImage.RenderTransform = rotateTransform;
        }

        public void RotateLeft()
        {
            var newValue = ImageRotation - 90;
            if (newValue < 0)
            {
                newValue = 270;
            }
            ImageRotation = newValue;

            var rotateTransform = new RotateTransform()
            {
                CenterX = PdfImage.ActualWidth / 2,
                CenterY = PdfImage.ActualHeight / 2,
                Angle = ImageRotation
            };
            PdfImage.RenderTransform = rotateTransform;
        }

        private void PdfImage_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (PreviewMode)
            {
                PdfPageClicked?.Invoke(this);
            }
        }
    }
}
