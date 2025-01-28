using Microsoft.Extensions.Logging;
using NotificationSystem.Business.Business;
using NotificationSystem.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Worker
{
    public abstract class WorkerBase<T>
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _memoryQueueProvider;
        protected readonly NotificationBO _notificationBO;
        protected readonly PeriodicTimer _timer;
        protected string _id = Guid.NewGuid().ToString();

        protected WorkerBase(ILoggerFactory loggerFactory, int sleepTimeInSeconds, ICacheProvider memoryQueueProvider, NotificationBO notificationBO) //TODO: Melhorar essa injeção
        {
            _logger = loggerFactory.CreateLogger(GetType()); //Using loggerFactory to create a ILogger instance in runtime
            _timer = new(TimeSpan.FromSeconds(sleepTimeInSeconds));
            _memoryQueueProvider = memoryQueueProvider;
            _notificationBO = notificationBO;
        }

        public abstract Task<List<T>> GetItemsToProcess();
        public abstract Task ProcessItem(T item);
        public abstract Task ProcessItemError(T item, Exception ex);

        //Confirmar com o pessoal qual a opinião sobre esse padrão de controle de periodicidade
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
