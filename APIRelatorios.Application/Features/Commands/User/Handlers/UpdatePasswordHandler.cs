using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class UpdatePasswordHandler
        : ICommandHandler<UpdatePasswordCommand>
{
    private readonly IPasswordHasher _passwordHasher;

    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    private readonly ILogger<UpdatePasswordHandler> _logger;

    public UpdatePasswordHandler(IUserCommands commands, IPasswordHasher passwordHasher, IUserQuery query, ILogger<UpdatePasswordHandler> logger)
    {
        _commands = commands;
        _passwordHasher = passwordHasher;
        _query = query;
        _logger = logger;
    }

    public async Task Handle(UpdatePasswordCommand updPassword, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando processo de atualização de senha para o usuário com ID {UserId}", updPassword.idUser);

        var user = await _query.BuscarFiscalId(updPassword.idUser)
            ?? throw new UserNotFoundException(updPassword.idUser);

        _logger.LogInformation("Usuário encontrado: {UserId}. Atualizando senha.", user.UserId);

        user.UpdatePassword(_passwordHasher.HashPassword(updPassword.Password, user.Salt));

        _logger.LogInformation("Senha atualizada para o usuário com ID {UserId}. Salvando alterações.", user.UserId);

        await _commands.UpdateUser(user);

        _logger.LogInformation("Processo de atualização de senha concluído para o usuário com ID {UserId}", user.UserId);
    }
}
