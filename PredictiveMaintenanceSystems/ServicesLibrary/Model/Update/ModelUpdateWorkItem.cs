using EFDataModels;
using ServicesLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model.Update
{
    public static class ModelUpdateWorkItem
    {
        public static void QueueModelUpdateWorkItem(this IBackgroundTaskQueue queue, Guid modelId)
        {
            queue.Queue(new ModelUpdateWorkOrder(modelId));
        }

        public class ModelUpdateWorkOrder : IBackgroundWorkOrder<ModelUpdateWorkOrder, ModelUpdateWorker>
        {
            public ModelUpdateWorkOrder(Guid modelId)
            {
                this.ModelId = modelId;
            }

            public Guid ModelId { get; }
        }

        public class ModelUpdateWorker : IBackgroundWorker<ModelUpdateWorkOrder, ModelUpdateWorker>
        {
            private EFSystemContext _context;
            private ModelTaskController _controller;

            public ModelUpdateWorker(EFSystemContext context)
            {
                _context = context;
                _controller = new ModelTaskController(_context);
            }

            public async Task DoWork(ModelUpdateWorkOrder order, CancellationToken cancellationToken)
            {
                await _controller.UpdateModel(order.ModelId, cancellationToken);
            }
        }
    }
}
