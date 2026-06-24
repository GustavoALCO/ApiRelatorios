using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class DeleteUsuarioHandler
        : ICommandHandler<DeleteUsuarioCommand>
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    private readonly IValidateIds _validateIds;

    private readonly ILogger<DeleteUsuarioHandler> _logger;

    public DeleteUsuarioHandler(IUserQuery query, IUserCommands commands, IValidateIds validateIds, ILogger<DeleteUsuarioHandler> logger)
    {
        _validateIds = validateIds;
        _query = query;
        _commands = commands;
        _logger = logger;
    }

    public async Task Handle(DeleteUsuarioCommand dltuser, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando processo de exclusão do usuário com ID {UserId}", dltuser.idUser);

        var user = await _query.BuscarFiscalId(dltuser.idUser)
            ?? throw new UserNotFoundException(dltuser.idUser);

        _logger.LogInformation("Usuário encontrado: {UserLogin} ({UserId})", user.Login, user.UserId);

        user.AlterValid(true);

        await _commands.DeleteUser(user);

        _logger.LogInformation("Usuário com ID {UserId} excluído com sucesso", dltuser.idUser);
    }
}
