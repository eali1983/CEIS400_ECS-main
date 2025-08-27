using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;

namespace CEIS400_ECS
{
    public class Barcode
    {
        public string Code { get; private set; }
        public Bitmap BarcodeImage { get; private set; }
        public Image Generate(string inputData)
        {
            Code = $"TID-{inputData}";

            var write = new BarcodeWriter<System.Drawing.Bitmap>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 100,
                    Width = 300,
                    Margin = 5
                }
            };

            BarcodeImage = write.Write(Code);
            return BarcodeImage;
        }
    }
}
