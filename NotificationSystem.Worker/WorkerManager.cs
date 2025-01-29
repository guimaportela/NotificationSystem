using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Infrastructure;
using NotificationSystem.Contracts.Worker;
using System.Collections.Concurrent;
using System.Reflection;

namespace NotificationSystem.Worker
{
    public class WorkerManager
    {
        private readonly ILogger<WorkerManager> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configs;


        public WorkerManager(ILogger<WorkerManager> logger, IServiceProvider serviceProvider, IConfiguration configs)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configs = configs;
        }

        public void SetupWorkers(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Running {nameof(SetupWorkers)}");

            //Getting workers configs in appsettings.json
            var workerConfigs = new List<WorkerConfigurations>();
            _configs.GetSection("Workers").Bind(workerConfigs);

            //Getting list of workers based on the base class
            var workers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass
                               && !type.IsGenericType
                               && (type.BaseType?.IsGenericType ?? false)
                               && type.BaseType.GetGenericTypeDefinition() == typeof(WorkerBase<>)).ToList();

            foreach (var workerConfiguration in workerConfigs)
            {
                Type workerType = workers.FirstOrDefault(type => type.Name == workerConfiguration.Name);

                if (workerType == null)
                {
                    _logger.LogError($"{workerConfiguration.Name} Worker Class not defined");
                    continue;
                }

                for (int i = 0; i < (workerConfiguration?.Instances ?? 1); i++)
                {
                    //Dinamically instantiating each worker instance using Reflection
                    dynamic _worker1 = Activator.CreateInstance(workerType, new object[]
                    {
                        _serviceProvider.GetService<ILoggerFactory>(),
                        workerConfiguration.SleepTimeInSeconds,
                        _serviceProvider.GetService<IMemoryQueueProvider>(),
                        _serviceProvider.GetService<INotificationBO>()
                    });

                    if (_worker1 == null)
                        continue;

                    //Starting Work execution defined in WorkerBase
                    _worker1.DoPeriodicWork(stoppingToken);
                }
            }
        }
    }
}
