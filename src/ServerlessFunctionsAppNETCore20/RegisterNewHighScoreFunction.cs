using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ServerlessFunctionsAppNETCore20.Orchestrations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessFunctionsAppNETCore20
{
    public class RegisterNewHighScoreFunction
    {
        [FunctionName("RegisterNewHighScore_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger logger)
        {
            // Function input comes from the request content.
            HighScore score = new HighScore() { Nickname = "LX360", Score = 1337 };
            string instanceId = await starter.StartNewAsync(nameof(RegisterNewHighScoreOrchestration), score);

            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(
                req, instanceId, TimeSpan.FromSeconds(5));
        }
    }
}
