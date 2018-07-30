using System;
using System.Threading.Tasks;

namespace MUtils.Cqrs {
    public sealed class QueryProcessor : IQueryProcessor {

        private readonly IServiceProvider _serviceProvider;

        public QueryProcessor(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public TResult Process<TResult>(IQuery<TResult> query) {
            var type = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var service = _serviceProvider.GetService(type);
            var func = QueryHandlersCache<IQuery<TResult>, TResult>.GetHandler(service.GetType());
            var res = func.Invoke(service, query);
            return res;
        }


        /// <inheritdoc />
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query) {
            var type = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var service = _serviceProvider.GetService(type);
            var func = AsyncQueryHandlersCache<IQuery<TResult>, TResult>.GetHandler(service.GetType());
            var res = func.Invoke(service, query);
            return res;
        }

    }
}