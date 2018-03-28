using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Village.Functions
{
    public class EmailTask
    {
        public string Name { get; set; }
    }
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run([QueueTrigger("email-queue", Connection = "AzureStorageConnectionString")]EmailTask myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
            await Task.Run(() =>
            {
                Thread.Sleep(100);
            });
        }
    }
}
