using ServicesLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model.Run
{
    public static class ModelRunWorkItem
    {
        public static void QueueModelRunWorkItem(this IBackgroundTaskQueue queue, Func<CancellationToken, Task> method)
        {
            queue.Queue(new ModelRunWorkOrder(method));
        }

        public class ModelRunWorkOrder : IBackgroundWorkOrder<ModelRunWorkOrder, ModelRunWorker>
        {
            public ModelRunWorkOrder(Func<CancellationToken, Task> method)
            {
                this.Method = method;
            }

            public Func<CancellationToken, Task> Method { get; }
        }

        public class ModelRunWorker : IBackgroundWorker<ModelRunWorkOrder, ModelRunWorker>
        {
            public async Task DoWork(ModelRunWorkOrder order, CancellationToken cancellationToken)
            {
                await order.Method.Invoke(cancellationToken);
            }
        }
    }
}
