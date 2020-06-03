using Xamarin.Forms;

namespace ERP.Client.Startup.PdfViewer
{
    public class PdfWebViewContentPage : ContentPage
    {
        public PdfWebViewContentPage()
        {
			Padding = new Thickness(0, 20, 0, 0);
			Content = new StackLayout
			{
				Children = {
					new PdfWebViewControl {
						Uri = "compressed.tracemonkey-pldi-09.pdf",
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand
					}
				}
			};
		}
    }
}
