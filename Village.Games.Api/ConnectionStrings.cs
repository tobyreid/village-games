using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Village.Games.Api
{
    public class ConnectionStrings
    {
        public string AzureStorageConnectionString { get; set; }
    }
    public class QueueResolver : IQueueResolver
    {
        private readonly CloudQueueClient _queueClient;
        public QueueResolver(IOptions<ConnectionStrings> settings)
        {
            var storageAccount = CloudStorageAccount.Parse(settings.Value.AzureStorageConnectionString);
            _queueClient = storageAccount.CreateCloudQueueClient();
        }

        public CloudQueue GetQueue(string queueName)
        {
            return _queueClient.GetQueueReference(queueName);
        }
    }

    public interface IQueueResolver
    {
        CloudQueue GetQueue(string queueName);
    }

    public class AzureQueues
    {
        public static string EmailQueue
        {
            get { return "email-queue"; }
        }
        public static IEnumerable<string> KnownQueues
        {
            get { return new[] {EmailQueue}; }
        }
    }
    public class BootstrapAzureQueues
    {
        public static void CreateKnownAzureQueues(string azureConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            foreach (var queueName in AzureQueues.KnownQueues)
            {
                AsyncHelper.RunSync(() => queueClient.GetQueueReference(queueName).CreateIfNotExistsAsync());
            }
        }
    }
    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
                .StartNew<Task<TResult>>(func)
                .Unwrap<TResult>()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
                .StartNew<Task>(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}
