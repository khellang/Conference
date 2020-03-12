using System.Threading;
using System.Threading.Tasks;

namespace Conference
{
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken);
    }
}