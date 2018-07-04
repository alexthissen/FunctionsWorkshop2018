using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ServerlessFunctionsAppNETCore20.Activities;

namespace ServerlessFunctionsAppNETCore20.Orchestrations
{
    public static class RegisterNewHighScoreOrchestration
    {
        [FunctionName(nameof(RegisterNewHighScoreOrchestration))]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContextBase context, 
            ILogger log)
        {
            var score = context.GetInput<HighScore>();

            var blobUri = await context.CallActivityAsync<string>(
                nameof(QRCodeGeneratorActivity),
                score);
            return blobUri;
        }
    }
}
