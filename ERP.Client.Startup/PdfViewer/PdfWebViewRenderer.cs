using ERP.Client.Startup.PdfViewer;
using System;
using System.Net;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(PdfWebViewControl), typeof(PdfWebViewRenderer))]
namespace ERP.Client.Startup.PdfViewer
{
    public class PdfWebViewRenderer
    {
        public class CustomWebViewRenderer : WebViewRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
            {
                base.OnElementChanged(e);

                if (e.NewElement != null)
                {
                    var pdfWebView = Element as PdfWebViewControl;
                    Control.Source = new Uri(string.Format("ms-appx-web:///Assets/PdfViewer/web/viewer.html?file={0}", string.Format("ms-appx-web:///Assets/Content/{0}", WebUtility.UrlEncode(pdfWebView.Uri))));
                }
            }
        }
    }
}
