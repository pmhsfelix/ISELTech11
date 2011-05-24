using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.ApplicationServer.Http;

namespace First
{
    class ImageFromTextFormatter : MediaTypeFormatter
    {

        public ImageFromTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
            //this.AddMediaRangeMapping("*/*", "audio/x-wav");
            this.AddUriPathExtensionMapping(".jpeg", "image/jpeg");
        }

        protected override bool OnCanReadType(Type type)
        {
            return false;
        }

        protected override bool OnCanWriteType(Type type)
        {
            return type == typeof (string);
        }

        public override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders)
        {
            throw new NotImplementedException();
        }

        public override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context)
        {
            var text = value as string;

            const int pts = 40;
            Bitmap bm = new Bitmap(text.Length * pts, 2 * pts);
            Graphics g = Graphics.FromImage(bm);
            g.DrawString(text, new Font("Arial", pts), Brushes.White, 1.0f, 1.0f);


            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            bm.Save(stream, jgpEncoder, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
