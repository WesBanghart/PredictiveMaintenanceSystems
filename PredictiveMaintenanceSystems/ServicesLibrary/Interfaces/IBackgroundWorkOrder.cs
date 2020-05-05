using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Interfaces
{
    // Code adapted from https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    /// <summary>
    /// Un-typed background work order interface. Used for building typed interfaces.
    /// </summary>
    public interface IBackgroundWorkOrder { }

    /// <summary>
    /// Typed background work order interface.
    /// </summary>
    /// <typeparam name="TWorkOrder">An IBackgroundWorkOrder type object.</typeparam>
    /// <typeparam name="TWorker">An IBackgroundWorker type object.</typeparam>
    public interface IBackgroundWorkOrder<TWorkOrder, TWorker> : IBackgroundWorkOrder
        where TWorker : IBackgroundWorker<TWorkOrder, TWorker>
        where TWorkOrder : IBackgroundWorkOrder<TWorkOrder, TWorker>
    {
    }
}
