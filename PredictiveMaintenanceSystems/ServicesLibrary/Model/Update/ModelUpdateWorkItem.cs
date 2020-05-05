using EFDataModels;
using ServicesLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model.Update
{
    // Code adapted from https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    /// <summary>
    /// Work item class for updating and training a model.
    /// </summary>
    public static class ModelUpdateWorkItem
    {
        /// <summary>
        /// Command to add a model update task to the background task queue.
        /// </summary>
        /// <param name="queue">The IBackgroundTaskQueue that the model update task should be added to.</param>
        /// <param name="modelId">The modelId relating to the database entry for the model to be updated.</param>
        public static void QueueModelUpdateWorkItem(this IBackgroundTaskQueue queue, Guid modelId)
        {
            queue.Queue(new ModelUpdateWorkOrder(modelId));
        }

        /// <summary>
        /// The model update work order class.
        /// </summary>
        public class ModelUpdateWorkOrder : IBackgroundWorkOrder<ModelUpdateWorkOrder, ModelUpdateWorker>
        {
            /// <summary>
            /// Constructor building a new model update work order for the given modelId.
            /// </summary>
            /// <param name="modelId">The modelId relating to the database entry for the model to be updated.</param>
            public ModelUpdateWorkOrder(Guid modelId)
            {
                this.ModelId = modelId;
            }

            /// <summary>
            /// The modelId relating to the database entry for the model to be updated.
            /// </summary>
            public Guid ModelId { get; }
        }

        /// <summary>
        /// The model update worker class
        /// </summary>
        public class ModelUpdateWorker : IBackgroundWorker<ModelUpdateWorkOrder, ModelUpdateWorker>
        {
            /// <summary>
            /// The database context
            /// </summary>
            private EFSystemContext _context;
            /// <summary>
            /// The model task controller
            /// </summary>
            private ModelTaskController _controller;

            /// <summary>
            /// Constructor building a new model update worker referencing the given database context.
            /// </summary>
            /// <param name="context">The context class for the target database.</param>
            public ModelUpdateWorker(EFSystemContext context)
            {
                _context = context;
                _controller = new ModelTaskController(_context);
            }

            /// <summary>
            /// The command that will perform the model update task asynchronously on a background thread.
            /// </summary>
            /// <param name="order">The model update work order that spawned this worker.</param>
            /// <param name="cancellationToken">The async task cancellation token.</param>
            /// <returns>The background task that will be performed.</returns>
            public async Task DoWork(ModelUpdateWorkOrder order, CancellationToken cancellationToken)
            {
                await _controller.UpdateModel(order.ModelId, cancellationToken);
            }
        }
    }
}
