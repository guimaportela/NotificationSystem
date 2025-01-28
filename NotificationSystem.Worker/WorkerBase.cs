using Microsoft.Extensions.Logging;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Infrastructure;

namespace NotificationSystem.Worker
{
    public abstract class WorkerBase<T>
    {
        protected readonly ILogger _logger;
        protected readonly IMemoryQueueProvider _memoryQueueProvider;
        protected readonly INotificationBO _notificationBO;
        protected readonly PeriodicTimer _timer;
        protected string _id = Guid.NewGuid().ToString();

        protected WorkerBase(ILoggerFactory loggerFactory, int sleepTimeInSeconds, IMemoryQueueProvider memoryQueueProvider, INotificationBO notificationBO) //TODO: Melhorar essa injeção
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _timer = new(TimeSpan.FromSeconds(sleepTimeInSeconds));
            _memoryQueueProvider = memoryQueueProvider;
            _notificationBO = notificationBO;
        }

        public abstract Task<List<T>> GetItemsToProcess();
        public abstract Task ProcessItem(T item);
        public abstract Task ProcessItemError(T item, Exception ex);

        internal async void DoPeriodicWork(CancellationToken stoppingToken)
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    var items = await GetItemsToProcess();

                    if (items.Count() == 0)
                        continue;

                    foreach (var item in items)
                    {
                        try
                        {
                            await ProcessItem(item);
                        }
                        catch (Exception ex)
                        {
                            await ProcessItemError(item, ex);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Timed Hosted Service is stopping.");
            }
        }
    }
}
