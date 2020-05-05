using ServicesLibrary.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model
{
    // Code adapted from https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    /// <summary>
    /// The background task queue implementation class.
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        /// <summary>
        /// The concurrent (thread-safe) queue for storing background work orders.
        /// </summary>
        private readonly ConcurrentQueue<IBackgroundWorkOrder> _workOrders = 
            new ConcurrentQueue<IBackgroundWorkOrder>();

        /// <summary>
        /// The semaphore used for controlling queue concurrency.
        /// </summary>
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        
        /// <summary>
        /// Command to remove the work order from the front of the queue.
        /// </summary>
        /// <param name="cancellationToken">The async cancellation token.</param>
        /// <returns>The background work order from the front of the queue</returns>
        public async Task<IBackgroundWorkOrder> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workOrders.TryDequeue(out var workItem);
            _signal.Release();

            return workItem;
        }

        /// <summary>
        /// Command to add a work order to the front of the queue
        /// </summary>
        /// <typeparam name="TWorkOrder">The type of the work order task being added.</typeparam>
        /// <typeparam name="TWorker">The type of the worker being added.</typeparam>
        /// <param name="order">The work order to add to the front of the queue.</param>
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
