using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Interfaces
{
    // Code adapted from https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    /// <summary>
    /// Interface for a queue that can handle background tasks through a work order/worker system.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Command to add a WorkOrder/Worker task pair to the back of the queue
        /// </summary>
        /// <typeparam name="TWorkOrder">An IBackgroundWorkOrder type object.</typeparam>
        /// <typeparam name="TWorker">An IBackgroundWorker type object.</typeparam>
        /// <param name="order">The WorkOrder task to add to the queue.</param>
        void Queue<TWorkOrder, TWorker>(IBackgroundWorkOrder<TWorkOrder, TWorker> order)
            where TWorker : IBackgroundWorker<TWorkOrder, TWorker>
            where TWorkOrder : IBackgroundWorkOrder<TWorkOrder, TWorker>;

        /// <summary>
        /// Command to remove the WorkOrder task from the front of the queue.
        /// </summary>
        /// <param name="cancellationToken">The async task cancellation token.</param>
        /// <returns>The WorkOrder task from the front of the queue.</returns>
        Task<IBackgroundWorkOrder> DequeueAsync(CancellationToken cancellationToken);
    }
}
