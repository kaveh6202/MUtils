using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MUtils.Cqrs {

    public class CommandLoggerDecorator<TCommand>
        : ICommandExecuter<TCommand> where TCommand : ICommand {

        private readonly ILogger _logger;
        private readonly ICommandExecuter<TCommand> _decoratee;

        public CommandLoggerDecorator(
            ILogger logger,
            ICommandExecuter<TCommand> decoratee) {
            _logger = logger;
            _decoratee = decoratee;
        }

        void ICommandExecuter<TCommand>.Execute(TCommand command)
        {
            var scopeId = Guid.NewGuid();
            _logger.Log(command.SessionId, scopeId, _decoratee, 0, command);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                _decoratee.Execute(command);
                stopwatch.Stop();
                _logger.Log(command.SessionId,scopeId, _decoratee, 1, command, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Log(command.SessionId, scopeId, _decoratee, 2, command, stopwatch.ElapsedMilliseconds, ex);
                 throw;
            }
        }

        Task ICommandExecuter<TCommand>.ExecuteAsync(TCommand command)
        {
            var scopeId = Guid.NewGuid();
            _logger.Log(command.SessionId, scopeId, _decoratee, 0, command);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                _decoratee.Execute(command); 
                stopwatch.Stop();
                _logger.Log(command.SessionId, scopeId, _decoratee, 1, command, stopwatch.ElapsedMilliseconds);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Log(command.SessionId, scopeId, _decoratee, 2, command, stopwatch.ElapsedMilliseconds, ex);
                throw;
            }
        }

        //public bool CanExecute(TCommand command)
        //{
        //    return _decoratee.CanExecute(command);
        //}
    }
}