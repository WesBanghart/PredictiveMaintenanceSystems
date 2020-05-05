using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Interfaces
{
    // Code adapted from https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    /// <summary>
    /// Un-typed background worker interface. Used for building typed interfaces.
    /// </summary>
    public interface IBackgroundWorker { }

    /// <summary>
    /// Typed background worker interface.
    /// </summary>
    /// <typeparam name="TWorkOrder">An IBackgroundWorkOrder type object.</typeparam>
    /// <typeparam name="TWorker">An IBackgroundWorker type object.</typeparam>
    public interface IBackgroundWorker<TWorkOrder, TWorker> : IBackgroundWorker
        where TWorker : IBackgroundWorker<TWorkOrder, TWorker>
        where TWorkOrder : IBackgroundWorkOrder<TWorkOrder, TWorker>
    {
        Task DoWork(TWorkOrder order, CancellationToken cancellationToken);
    }
}
