using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServicesLibrary.Interfaces;

namespace ModelUpdateQueueHostedService
{
    //Following https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    public class ModelUpdateQueueWorker : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly CancellationTokenSource _shutdown = new CancellationTokenSource();
        private readonly ILogger<ModelUpdateQueueWorker> _logger;
        private Task _backgroundTask;

        public IBackgroundTaskQueue TaskQueue { get; }

        public ModelUpdateQueueWorker(IServiceProvider services,
                                      IBackgroundTaskQueue taskQueue,
                                      ILogger<ModelUpdateQueueWorker> logger)
        {
            _services = services;
            TaskQueue = taskQueue;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            _backgroundTask = Task.Run(BackgroundProceessing);

            return Task.CompletedTask;
        }

        private async Task BackgroundProceessing()
        {
            while (!_shutdown.IsCancellationRequested)
            {
                var workOrder = await TaskQueue.DequeueAsync(_shutdown.Token);

                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var workerType = workOrder
                            .GetType()
                            .GetInterfaces()
                            .First(t => t.IsConstructedGenericType &&
                                t.GetGenericTypeDefinition() == typeof(IBackgroundWorkOrder<,>))
                            .GetGenericArguments()
                            .Last();

                        var worker = scope.ServiceProvider
                            .GetRequiredService(workerType);

                        var task = (Task)workerType.GetMethod("DoWork")
                            .Invoke(worker, new object[] { workOrder, _shutdown.Token });

                        await task;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred executing {nameof(workOrder)}.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            _shutdown.Cancel();

            return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
