using System.Threading.Tasks;

namespace MUtils.CqrsBase {
    public interface ICommandExecuter<in TCommand> where TCommand : ICommand {

        void Execute(TCommand command);
        Task ExecuteAsync(TCommand command);
        //bool CanExecute(TCommand command);
    }
    
}