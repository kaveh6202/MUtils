using System.Threading.Tasks;

namespace MUtils.Cqrs {
    public interface IQueryHandler<in TQuery, TResult>
    {
        TResult Handle(TQuery query);
        Task<TResult> HandleAsync(TQuery query);
    }
}