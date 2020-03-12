using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Conference
{
    public class ValidationCommandBus : ICommandBus
    {
        public ValidationCommandBus(ICommandBus inner, IServiceProvider provider)
        {
            Inner = inner;
            Provider = provider;
        }

        private ICommandBus Inner { get; }

        private IServiceProvider Provider { get; }

        public ValueTask<TResult> Handle<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand<TResult>
        {
            var validator = Provider.GetService<IValidator<TCommand>>();

            if (validator is null)
            {
                return Inner.Handle<TCommand, TResult>(command, cancellationToken);
            }

            return HandleValidation<TCommand, TResult>(command, validator, cancellationToken);
        }

        private async ValueTask<TResult> HandleValidation<TCommand, TResult>(TCommand command, IValidator<TCommand> validator, CancellationToken cancellationToken)
            where TCommand : ICommand<TResult>
        {
            var result = await validator.ValidateAsync(command, cancellationToken);

            if (result.IsValid)
            {
                return await Inner.Handle<TCommand, TResult>(command, cancellationToken);
            }

            throw new ValidationException(result.Errors);
        }
    }
}
