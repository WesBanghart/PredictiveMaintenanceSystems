using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Interfaces
{
    //Following https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    public interface IBackgroundTaskQueue
    {
        void Queue<TWorkOrder, TWorker>(IBackgroundWorkOrder<TWorkOrder, TWorker> order)
            where TWorker : IBackgroundWorker<TWorkOrder, TWorker>
            where TWorkOrder : IBackgroundWorkOrder<TWorkOrder, TWorker>;

        Task<IBackgroundWorkOrder> DequeueAsync(CancellationToken cancellationToken);
    }
}
