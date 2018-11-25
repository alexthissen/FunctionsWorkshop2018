using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Text;
using System;
using Microsoft.Extensions.Logging;

namespace ServerlessFunctionsAppNETCore
{
    public static class DumpHeadersFunction
    {
        [FunctionName("DumpHeadersFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest request, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var builder = new StringBuilder();
            foreach (var header in request.Headers)
            {
                builder.AppendFormat("{0}='{1}',", header.Key, String.Concat(header.Value));
            }

            return new OkObjectResult(builder.ToString());
        }
    }
}
