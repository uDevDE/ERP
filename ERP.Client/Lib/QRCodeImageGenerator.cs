using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ERP.Client.Lib
{
    public class QRCodeImageGenerator
    {
        public static async Task<BitmapImage> Generate(string data, int pixelPerModule = 8, QRCodeGenerator.ECCLevel eCCLevel = QRCodeGenerator.ECCLevel.Q)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, eCCLevel);

            PngByteQRCode qrCodePng = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImagePng = qrCodePng.GetGraphic(8);
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(qrCodeImagePng);
                    await writer.StoreAsync();
                }
                var image = new BitmapImage();
                await image.SetSourceAsync(stream);

                return image;
            }
        }
    }
}
