using System.Threading;
using System.Threading.Tasks;

namespace Conference
{
    public interface IQueryExecutor
    {
        Task<TResult> Execute<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery<TResult>;
    }

    public interface ICommandBus
    {
        ValueTask<TResult> Handle<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand<TResult>;
    }
}