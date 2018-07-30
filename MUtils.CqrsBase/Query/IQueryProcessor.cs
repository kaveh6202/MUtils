using System.Threading.Tasks;

namespace MUtils.CqrsBase {
    public interface IQueryProcessor {
        TResult Process<TResult>(IQuery<TResult> query);
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);
    }
}