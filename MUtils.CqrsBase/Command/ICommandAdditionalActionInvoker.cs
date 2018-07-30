using System;

namespace MUtils.CqrsBase
{
    public interface ICommandAdditionalActionInvoker<in TCommand>
    {
        void PreAction(TCommand command);
        void OnSuccess(TCommand command);
        void OnFail(TCommand command, Exception ex);
    }
}