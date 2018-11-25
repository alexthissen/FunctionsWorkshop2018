using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ServerlessFunctionsAppNETCore
{
    public static class ImportHighScoreFunction
    {
        [FunctionName("ImportHighScoreFunction")]
        public static async Task Run(
            [QueueTrigger("azurefunctions-import", Connection = "azurefunctions-queues")] HighScoreEntry[] entries,
            ILogger log,
            [Table("highscores", Connection = "azurefunctions-tables")] IAsyncCollector<HighScoreTableItem> table)
        {
            log.LogInformation("C# Queue trigger function processed");

            foreach (HighScoreEntry entry in entries)
            {
                await table.AddAsync(new HighScoreTableItem()
                {
                    PartitionKey = entry.Game,
                    RowKey = Guid.NewGuid().ToString(),
                    Nickname = entry.Nickname,
                    Score = entry.Score
                });
            }
        }

        public class HighScoreEntry
        {
            public string Game { get; set; }
            public string Nickname { get; set; }
            public int Score { get; set; }
        }

        public class HighScoreTableItem
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }

            public string Nickname { get; set; }
            public int Score { get; set; }
        }

    }
}
