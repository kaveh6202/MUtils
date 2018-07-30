using System;
using System.Threading.Tasks;

namespace MUtils.CqrsBase
{
    public class CommandAdditionalActionsDecorator<TCommand> : ICommandExecuter<TCommand> where TCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly ICommandExecuter<TCommand> _decoratee;
        private readonly ICommandAdditionalActionInvoker<TCommand> _actions;

        public CommandAdditionalActionsDecorator(ICommandExecuter<TCommand> decoratee, ILogger logger,
            ICommandAdditionalActionInvoker<TCommand> actions)
        {
            _decoratee = decoratee;
            _logger = logger;
            this._actions = actions;
        }

        public void Execute(TCommand command)
        {
            try
            {
                _actions.PreAction(command);
                _decoratee.Execute(command);
                _actions.OnSuccess(command);
            }
            catch (Exception ex)
            {
                _actions.OnFail(command, ex);
                //throw;
            }
            finally
            {
                
            }
        }

        public async Task ExecuteAsync(TCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}