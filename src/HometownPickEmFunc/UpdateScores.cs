using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace HometownPickEmFunc
{
    public static class UpdateScores
    {
        [FunctionName("UpdateScoresNightly")]
        public static async Task NightlyUpdatesAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
        }

        [FunctionName("UpdateScoresWeekend")]
        public static async Task SaturdayUpdatesAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
        }
    }
}