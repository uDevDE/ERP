using Xamarin.Forms;

namespace ERP.Client.Startup.PdfViewer
{
    public class PdfWebViewControl : WebView
	{
		public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
		returnType: typeof(string),
		declaringType: typeof(PdfWebViewControl),
		defaultValue: default(string));

		public string Uri
		{
			get { return (string)GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}
	}
}
