using System;
using System.Linq.Expressions;

namespace MUtils.Cqrs {
    internal static class QueryHandlersCache<TQuery, TResult> where TQuery : IQuery<TResult> {

        #region GetHandler1
        /*
        .Lambda #Lambda1<System.Func`3[System.Object,MUtils.Cqrs.IQuery`1[System.Nullable`1[System.Int64]],System.Nullable`1[System.Int64]]>(
            System.Object $handler,
            MUtils.Cqrs.IQuery`1[System.Nullable`1[System.Int64]] $query) {
            .Block(MUtils.Cqrs.Tests.GetUserIdQueryHandler $instance) {
                $instance = (MUtils.Cqrs.Tests.GetUserIdQueryHandler)$handler;
                .Call $instance.Handle((MUtils.Cqrs.Tests.GetUserIdQuery)$query)
            }
        }
        */
        public static Func<object, TQuery, TResult> GetHandler1(Type handlerType) {

            var methodInfo = handlerType.GetMethod(nameof(IQueryHandler<TQuery, TResult>.Handle));

            var handlerExp = Expression.Parameter(typeof(object), "handler");

            var inputExp = Expression.Parameter(typeof(TQuery), "query");

            var instanceExp = Expression.Variable(handlerType, "instance");

            var assExp = Expression.Assign(instanceExp, Expression.Convert(handlerExp, handlerType));

            var castExp = Expression.Convert(inputExp, methodInfo.GetParameters()[0].ParameterType);

            var callExp = Expression.Call(instanceExp, methodInfo, castExp);

            var blockExp = Expression.Block(new[] { instanceExp }, new Expression[] {
                assExp,
                callExp
            });

            var lambda = Expression.Lambda<Func<object, TQuery, TResult>>(
                blockExp,
                handlerExp,
                inputExp);

            var func = lambda.Compile();
            return func;
        }
        #endregion


        #region GetHandler

        private static Func<object, TQuery, TResult> _cache;

        public static Func<object, TQuery, TResult> GetHandler(Type handlerType) {
            if (_cache != null)
                return _cache;
            _cache = GetHandlerInternal(handlerType);
            return _cache;
        }


        /*
        .Lambda #Lambda1<System.Func`3[System.Object,MUtils.Cqrs.IQuery`1[System.Nullable`1[System.Int64]],System.Nullable`1[System.Int64]]>(
            System.Object $handler,
            MUtils.Cqrs.IQuery`1[System.Nullable`1[System.Int64]] $query) {
            .Call ((MUtils.Cqrs.Tests.GetUserIdQueryHandler)$handler).Handle((MUtils.Cqrs.Tests.GetUserIdQuery)$query)
        } 
        */
        private static Func<object, TQuery, TResult> GetHandlerInternal(Type handlerType) {

            var methodInfo = handlerType.GetMethod(nameof(IQueryHandler<TQuery, TResult>.Handle));

            var handlerExp = Expression.Parameter(typeof(object), "handler");

            var inputExp = Expression.Parameter(typeof(TQuery), "query");

            var callExp = Expression.Call(
                Expression.Convert(handlerExp, handlerType),
                methodInfo,
                Expression.Convert(inputExp, methodInfo.GetParameters()[0].ParameterType));

            var lambda = Expression.Lambda<Func<object, TQuery, TResult>>
                (callExp, handlerExp, inputExp);

            var func = lambda.Compile();
            return func;
        }
        #endregion
    }
}