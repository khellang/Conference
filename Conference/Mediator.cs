using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;

namespace Conference
{
    public class Mediator : IQueryExecutor, ICommandBus
    {
        public Mediator(IServiceProvider provider)
        {
            Provider = provider;
        }

        private IServiceProvider Provider { get; }

        public ValueTask<TResult> Handle<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand<TResult>
        {
            var handler = Provider.GetService<ICommandHandler<TCommand, TResult>>();

            if (handler is null)
            {
                var displayName = TypeNameHelper.GetTypeDisplayName(typeof(TCommand), fullName: false);
                throw new InvalidOperationException($"Could not find handler for command of type '{displayName}'.");
            }

            return handler.Handle(command, cancellationToken);
        }

        public Task<TResult> Execute<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery<TResult>
        {
            var handler = Provider.GetService<IQueryHandler<TQuery, TResult>>();

            if (handler is null)
            {
                var displayName = TypeNameHelper.GetTypeDisplayName(typeof(TQuery), fullName: false);
                throw new InvalidOperationException($"Could not find handler for query of type '{displayName}'.");
            }

            return handler.Handle(query, cancellationToken);
        }
    }
}
