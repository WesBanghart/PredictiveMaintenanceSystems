using ServicesLibrary.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<IBackgroundWorkOrder> _workOrders = 
            new ConcurrentQueue<IBackgroundWorkOrder>();

        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<IBackgroundWorkOrder> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workOrders.TryDequeue(out var workItem);
            _signal.Release();

            return workItem;
        }

        public void Queue<TWorkOrder, TWorker>(IBackgroundWorkOrder<TWorkOrder, TWorker> order)
            where TWorkOrder : IBackgroundWorkOrder<TWorkOrder, TWorker>
            where TWorker : IBackgroundWorker<TWorkOrder, TWorker>
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _signal.Wait();
            _workOrders.Enqueue(order);
            _signal.Release();
        }
    }
}
