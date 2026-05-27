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
        var handler = _serviceProvider
            .GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        return await handler.Handle(query, cancellationToken);
    }

    public async Task Send<Tcommand>(
        Tcommand command,
        CancellationToken cancellationToken = default)
        where Tcommand : ICommand
    {
        var handler = _serviceProvider
            .GetRequiredService<ICommandHandler<Tcommand>>();

        await handler.Handle(command, cancellationToken);
    }

    public async Task<TResponse> Send<Tcommand, TResponse>(
        Tcommand command,
        CancellationToken cancellationToken = default)
        where Tcommand : ICommand<TResponse>
    {
        var handler = _serviceProvider
            .GetRequiredService<ICommandHandler<Tcommand, TResponse>>();

        return await handler.Handle(command, cancellationToken);
    }
}