using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MUtils.CqrsBase
{
    public class QueryLoggerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly ILogger _logger;
        private readonly IQueryHandler<TQuery, TResult> _decoratee;
        public QueryLoggerDecorator(ILogger logger, IQueryHandler<TQuery, TResult> decoratee)
        {
            _logger = logger;
            _decoratee = decoratee;
        }
        TResult IQueryHandler<TQuery, TResult>.Handle(TQuery query)
        {
            var scopeId = Guid.NewGuid();
            _logger.Log(query.SessionId,scopeId, _decoratee, 0, query);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var response = _decoratee.Handle(query);
                stopwatch.Stop();
                _logger.Log(query.SessionId, scopeId, _decoratee, 1, response, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                _logger.Log(query.SessionId, scopeId, _decoratee, 2, null, stopwatch.ElapsedMilliseconds, e);
                throw;
            }
        }

        Task<TResult> IQueryHandler<TQuery, TResult>.HandleAsync(TQuery query)
        {
            var scopeId = Guid.NewGuid();
            _logger.Log(query.SessionId, scopeId, _decoratee, 0, query);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var response = _decoratee.Handle(query);
                stopwatch.Stop();
                _logger.Log(query.SessionId, scopeId, _decoratee, 1, response, stopwatch.ElapsedMilliseconds);
                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                _logger.Log(query.SessionId, scopeId, _decoratee, 2, null, stopwatch.ElapsedMilliseconds, e);
                throw;
            }
        }
    }
}