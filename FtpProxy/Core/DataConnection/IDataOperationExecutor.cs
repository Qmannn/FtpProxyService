using System;

namespace FtpProxy.Core.DataConnection
{
    public interface IDataOperationExecutor
    {
        void DoDataConnectionOperation(IAsyncResult result);
    }
}