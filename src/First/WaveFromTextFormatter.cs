using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Speech.Synthesis;
using System.Text;
using Microsoft.ApplicationServer.Http;

namespace First
{
    class WaveFromTextFormatter : MediaTypeFormatter
    {

        public WaveFromTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("audio/x-wav"));
            //this.AddMediaRangeMapping("*/*", "audio/x-wav");
            this.AddUriPathExtensionMapping(".wav","audio/x-wav");
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
            var s = value as string;
            if (s == null) return;
            using (var synth = new SpeechSynthesizer())
            {
                var ms = new MemoryStream();
                synth.SetOutputToWaveStream(ms);
                //synth.Rate -= 10;
                synth.Speak("current time is " + s);
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(stream);
            }   
        }
    }
}
