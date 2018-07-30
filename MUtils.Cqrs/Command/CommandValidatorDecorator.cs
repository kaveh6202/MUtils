using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MUtils.Cqrs {

    public class CommandValidatorDecorator<TCommand>
        : ICommandExecuter<TCommand> where TCommand : ICommand {

        private readonly IValidator<TCommand> _validator;
        private readonly ILogger _logger;
        private readonly ICommandExecuter<TCommand> _decoratee;

        public CommandValidatorDecorator(
            IValidator<TCommand> validator,
            ICommandExecuter<TCommand> decoratee, ILogger logger)
        {
            _validator = validator;
            _decoratee = decoratee;
            _logger = logger;
        }

        void ICommandExecuter<TCommand>.Execute(TCommand command) {
            _validator.ValidateObject(command);
            var scopeId = Guid.NewGuid();
            _logger.Log(command.SessionId,scopeId, _decoratee, 0, command);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                _decoratee.Execute(command);
                stopwatch.Stop();
                _logger.Log(command.SessionId, scopeId, _decoratee, 1, command, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Log(command.SessionId, scopeId, _decoratee, 2, command, stopwatch.ElapsedMilliseconds, ex);
                throw;
            }
            //_decoratee.Execute(command);
        }

        Task ICommandExecuter<TCommand>.ExecuteAsync(TCommand command) {
            _validator.ValidateObject(command);
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
