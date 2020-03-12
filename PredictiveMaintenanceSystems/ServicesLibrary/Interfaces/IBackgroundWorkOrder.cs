using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Interfaces
{
    //Following https://github.com/dotnet/extensions/issues/805#issuecomment-399864251
    public interface IBackgroundWorkOrder { }

    public interface IBackgroundWorkOrder<TWorkOrder, TWorker> : IBackgroundWorkOrder
        where TWorker : IBackgroundWorker<TWorkOrder, TWorker>
        where TWorkOrder : IBackgroundWorkOrder<TWorkOrder, TWorker>
    {
    }
}
