using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace First
{
    class SampleMessageHandler : DelegatingChannel
    {
        public SampleMessageHandler(HttpMessageChannel innerChannel) : base(innerChannel)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            return
                base.SendAsync(request, cancellationToken)
                    .ContinueWith(task =>
                                      {
                                          sw.Stop();
                                          task.Result.Headers.Add("X-Elapsed-Time",sw.ElapsedMilliseconds.ToString());
                                          return task.Result;
                                      },TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
