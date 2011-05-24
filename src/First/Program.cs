using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace First
{
    class Program
    {
        static void Main(String[] args)
        {
            //SimpleMain();
            MainWithConfiguration();
        }
        static void SimpleMain()
        {
            using (var host = new HttpServiceHost(typeof(TheService), "http://localhost:8080"))
            {
                host.Open();
                Console.WriteLine("Host opened at {0} , press any key to end", host.Description.Endpoints[0].Address);
                Console.ReadKey();
            }
        }

        static void MainWithConfiguration()
        {
            var cfg = HttpHostConfiguration.Create()
                .AddMessageHandlers(typeof (SampleMessageHandler))
                .SetOperationHandlerFactory(new MyFactory());
                
                
            
            using (var host = new HttpConfigurableServiceHost(typeof(TheService), cfg, new Uri("http://localhost:8080")))
            {
                host.Open();
                Console.WriteLine("Host opened at {0} , press any key to end", host.Description.Endpoints[0].Address);
                Console.ReadKey();
            }
        }
    }

    internal class MyFactory : HttpOperationHandlerFactory
    {
        
        protected override Collection<HttpOperationHandler> OnCreateRequestHandlers(ServiceEndpoint endpoint, HttpOperationDescription operation)
        {
            var coll = base.OnCreateRequestHandlers(endpoint, operation);
            coll.Add(new LoggingOperationHandler(operation));
            if (operation.Name == "GetTimeString")
            {
                Formatters.Remove(Formatters.XmlFormatter);
                Formatters.Remove(Formatters.JsonFormatter);
                Formatters.Add(new WaveFromTextFormatter());
                Formatters.Add(new ImageFromTextFormatter());
            }


            return coll;
        }
        
    }
}
