using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using Microsoft.ApplicationServer.Http;

namespace First
{
    [ServiceContract]
    internal class TheService
    {
        [WebGet(UriTemplate = "time")]
        private HttpResponseMessage TheOperation(HttpRequestMessage req)
        {
            return new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(DateTime.Now.ToLongTimeString())
                       };
        }

        [WebGet(UriTemplate = "time/{zone}")]
        public HttpResponseMessage GetTimeForZone(HttpRequestMessage req, string zone)
        {
            try
            {
                var zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);
                return new HttpResponseMessage()
                           {
                               StatusCode = HttpStatusCode.OK,
                               Content =
                                   new StringContent(TimeZoneInfo.ConvertTime(DateTime.Now, zoneInfo).ToLongTimeString())
                           };
            }
            catch (TimeZoneNotFoundException e)
            {
                return new HttpResponseMessage()
                           {
                               StatusCode = HttpStatusCode.NotFound,
                               Content = new StringContent(e.Message)
                           };
            }
        }

        [WebGet(UriTemplate = "zones.txt")]
        public HttpResponseMessage GetZonesInText()
        {
            var sb = new StringBuilder();
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                sb.AppendLine(string.Format("{0}: {1}", tz.Id, tz.StandardName));
            }
            return new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(sb.ToString())
                       };
        }

        [WebGet(UriTemplate="delay/{ms}")]
        public HttpResponseMessage Delay(int ms)
        {
            Thread.Sleep(ms);
            return new HttpResponseMessage(HttpStatusCode.OK,"done");
        }

        [WebGet(UriTemplate = "time2/*")]
        public string GetTimeString(HttpRequestMessage req)
        {
            
            return DateTime.Now.ToLongTimeString();
        }
    }
}