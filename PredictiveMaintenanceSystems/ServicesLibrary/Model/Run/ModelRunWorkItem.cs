using EFDataModels;
using ServicesLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model.Run
{
    // Code adapted from https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    /// <summary>
    /// Work item class for running a model.
    /// </summary>
    public static class ModelRunWorkItem
    {
        /// <summary>
        /// Command to add a model run task to the background task queue.
        /// </summary>
        /// <param name="queue">The IBackgroundTaskQueue that the model run task should be added to.</param>
        /// <param name="modelId">The modelId relating to the database entry for the model to be run.</param>
        public static void QueueModelRunWorkItem(this IBackgroundTaskQueue queue, Guid modelId)
        {
            queue.Queue(new ModelRunWorkOrder(modelId));
        }

        /// <summary>
        /// The model run work order class.
        /// </summary>
        public class ModelRunWorkOrder : IBackgroundWorkOrder<ModelRunWorkOrder, ModelRunWorker>
        {
            /// <summary>
            /// Constructor building a new model run work order for the given modelId.
            /// </summary>
            /// <param name="modelId">The modelId relating to the database entry for the model to be run.</param>
            public ModelRunWorkOrder(Guid modelId)
            {
                this.ModelId = modelId;
            }

            /// <summary>
            /// The modelId relating to the database entry for the model to be run.
            /// </summary>
            public Guid ModelId { get; }
        }

        /// <summary>
        /// The model run worker class
        /// </summary>
        public class ModelRunWorker : IBackgroundWorker<ModelRunWorkOrder, ModelRunWorker>
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
            /// Constructor building a new model run worker referencing the given database context.
            /// </summary>
            /// <param name="context">The context class for the target database.</param>
            public ModelRunWorker(EFSystemContext context)
            {
                _context = context;
                _controller = new ModelTaskController(_context);
            }

            /// <summary>
            /// The command that will perform the model run task asynchronously on a background thread.
            /// </summary>
            /// <param name="order">The model run work order that spawned this worker.</param>
            /// <param name="cancellationToken">The async task cancellation token.</param>
            /// <returns>The background task that will be performed.</returns>
            public async Task DoWork(ModelRunWorkOrder order, CancellationToken cancellationToken)
            {
                await _controller.RunModel(order.ModelId, cancellationToken);
            }
        }
    }
}
