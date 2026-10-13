using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace APIRelatorios.Application.Abstractions.Messaging;

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Query<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
    {
        await ValidateAsync(query, cancellationToken);

        var handler = _serviceProvider
            .GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        return await handler.Handle(query, cancellationToken);
    }

    public async Task Send<Tcommand>(
        Tcommand command,
        CancellationToken cancellationToken = default)
        where Tcommand : ICommand
    {
        await ValidateAsync(command, cancellationToken);

        var handler = _serviceProvider
            .GetRequiredService<ICommandHandler<Tcommand>>();

        await handler.Handle(command, cancellationToken);
    }

    public async Task<TResponse> Send<Tcommand, TResponse>(
        Tcommand command,
        CancellationToken cancellationToken = default)
        where Tcommand : ICommand<TResponse>
    {
        await ValidateAsync(command, cancellationToken);

        var handler = _serviceProvider
            .GetRequiredService<ICommandHandler<Tcommand, TResponse>>();

        return await handler.Handle(command, cancellationToken);
    }

    private async Task ValidateAsync<T>(T instance, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();

        if (validator is null)
            return;

        var result = await validator.ValidateAsync(instance, cancellationToken);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
