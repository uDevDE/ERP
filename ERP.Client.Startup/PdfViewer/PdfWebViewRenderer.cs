using ERP.Client.Startup.PdfViewer;
using System;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(PdfWebViewControl), typeof(PdfWebViewRenderer))]
namespace ERP.Client.Startup.PdfViewer
{
    public class PdfWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                //var pdfWebView = Element as PdfWebViewControl;
                Control.Source = new Uri(string.Format("ms-appx-web:///Assets/PdfViewer/web/viewer.html"));
                // string.Format("ms-appx-web:///Assets/Content/{0}", WebUtility.UrlEncode(pdfWebView.Uri))
                Control.LoadCompleted += Control_LoadCompleted;
            }
        }

        private async void Control_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            PdfWebViewControl pdfView = Element as PdfWebViewControl;
            if (string.IsNullOrEmpty(pdfView?.Uri)) return;
            try
            {
                var Base64Data = await OpenAndConvert(pdfView?.Uri);
                var obj = await Control.InvokeScriptAsync("openPdfAsBase64", new[] { Base64Data });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private async Task<string> OpenAndConvert(string FileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(FileName);
            var filebuffer = await file.OpenAsync(FileAccessMode.Read);
            var reader = new DataReader(filebuffer.GetInputStreamAt(0));
            var bytes = new byte[filebuffer.Size];
            await reader.LoadAsync((uint)filebuffer.Size);
            reader.ReadBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

    }
}
