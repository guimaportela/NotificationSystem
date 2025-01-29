using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace NotificationSystem.Worker
{
    public class WorkerBackgroundService : BackgroundService
    {
        private readonly ILogger<WorkerBackgroundService> _logger;
        private readonly WorkerManager _workerManager;

        private int _executionCount;

        public WorkerBackgroundService(ILogger<WorkerBackgroundService> logger, WorkerManager workerManager)
        {
            _logger = logger;
            _workerManager = workerManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkerBackgroundService: Running.");

            try
            {
                _workerManager.SetupWorkers(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }
    }
}
