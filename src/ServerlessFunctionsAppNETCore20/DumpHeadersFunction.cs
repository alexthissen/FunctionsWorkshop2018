
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Text;
using System;

namespace ServerlessFunctionsAppNETCore20
{
    public static class DumpHeadersFunction
    {
        [FunctionName("DumpHeadersFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest request, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            StringBuilder builder = new StringBuilder();
            foreach (var header in request.Headers)
            {
                builder.AppendFormat("{0}='{1}',", header.Key, String.Concat(header.Value));
            }

            return new OkObjectResult(builder.ToString());
        }
    }
}
