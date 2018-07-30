using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MUtils.CqrsBase {
    internal static class AsyncQueryHandlersCache<TQuery, TResult> where TQuery : IQuery<TResult> {

        #region GetHandler

        private static Func<object, TQuery, Task<TResult>> _cache;

        public static Func<object, TQuery, Task<TResult>> GetHandler(Type handlerType) {
            if (_cache != null)
                return _cache;
            _cache = GetHandlerInternal(handlerType);
            return _cache;
        }


        /*
        .Lambda #Lambda1<System.Func`3[System.Object,MUtils.CqrsBase.IQuery`1[System.Nullable`1[System.Int64]],System.Nullable`1[System.Int64]]>(
            System.Object $handler,
            MUtils.CqrsBase.IQuery`1[System.Nullable`1[System.Int64]] $query) {
            .Call ((MUtils.CqrsBase.Tests.GetUserIdQueryHandler)$handler).Handle((MUtils.CqrsBase.Tests.GetUserIdQuery)$query)
        } 
        */
        private static Func<object, TQuery, Task<TResult>> GetHandlerInternal(Type handlerType) {

            var methodInfo = handlerType.GetMethod(nameof(IQueryHandler<TQuery, TResult>.HandleAsync));

            var handlerExp = Expression.Parameter(typeof(object), "handler");

            var inputExp = Expression.Parameter(typeof(TQuery), "query");

            var callExp = Expression.Call(
                Expression.Convert(handlerExp, handlerType),
                methodInfo,
                Expression.Convert(inputExp, methodInfo.GetParameters()[0].ParameterType));

            var lambda = Expression.Lambda<Func<object, TQuery, Task<TResult>>>
                (callExp, handlerExp, inputExp);

            var func = lambda.Compile();
            return func;
        }
        #endregion
    }
}