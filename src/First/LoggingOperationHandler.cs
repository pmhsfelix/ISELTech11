using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace First
{
    class LoggingOperationHandler : HttpOperationHandler
    {
        private readonly HttpOperationDescription _desc;

        public LoggingOperationHandler(HttpOperationDescription desc)
        {
            _desc = desc;
        }

        protected override IEnumerable<HttpParameter> OnGetInputParameters()
        {
            return _desc.InputParameters;
        }

        protected override IEnumerable<HttpParameter> OnGetOutputParameters()
        {
            yield break;
        }

        protected override object[] OnHandle(object[] input)
        {
            Console.WriteLine("{");
            for (int i = 0; i < input.Length; ++i)
            {
                Console.WriteLine("    {0}:{1}", _desc.InputParameters[i].Name, input[i]);
            }
            Console.WriteLine("}");
            return new object[0];
        }
    }
}
