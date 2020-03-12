using EFDataModels;
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
        public static void QueueModelRunWorkItem(this IBackgroundTaskQueue queue, Guid modelId)
        {
            queue.Queue(new ModelRunWorkOrder(modelId));
        }

        public class ModelRunWorkOrder : IBackgroundWorkOrder<ModelRunWorkOrder, ModelRunWorker>
        {
            public ModelRunWorkOrder(Guid modelId)
            {
                this.ModelId = modelId;
            }

            public Guid ModelId { get; }
        }

        public class ModelRunWorker : IBackgroundWorker<ModelRunWorkOrder, ModelRunWorker>
        {
            private EFSystemContext _context;
            private ModelTaskController _controller;

            public ModelRunWorker(EFSystemContext context)
            {
                _context = context;
                _controller = new ModelTaskController(_context);
            }

            public async Task DoWork(ModelRunWorkOrder order, CancellationToken cancellationToken)
            {
                await _controller.RunModel(order.ModelId, cancellationToken);
            }
        }
    }
}
