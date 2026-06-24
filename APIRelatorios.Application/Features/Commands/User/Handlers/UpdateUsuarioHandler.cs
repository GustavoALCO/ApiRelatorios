using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class UpdateUsuarioHandler
        : ICommandHandler<AlterarUsuarioCommand>
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    private readonly IValidateIds _validateIds;

    private readonly ILogger<UpdateUsuarioHandler> _logger;

    public UpdateUsuarioHandler(IUserQuery query, IUserCommands commands, IValidateIds validateIds, ILogger<UpdateUsuarioHandler> logger)
    {
        _query = query;
        _commands = commands;
        _validateIds = validateIds;
        _logger = logger;
    }

    public async Task Handle(AlterarUsuarioCommand alterUser, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando processo de atualização do usuário com ID {UserId}", alterUser.userId);

        var user = await _query.BuscarFiscalId(alterUser.userId)
            ?? throw new UserNotFoundException(alterUser.userId);

        _logger.LogInformation("Usuário encontrado: {UserLogin}", user.Login);

        if (!string.IsNullOrEmpty(alterUser.login))
        {
            _logger.LogInformation("Atualizando login do usuário ID {UserId} para {NewLogin}", alterUser.userId, alterUser.login);
            user.UpdateLogin(alterUser.login);
        }
            

        if (!string.IsNullOrEmpty(alterUser.nome) || !string.IsNullOrEmpty(alterUser.sobrenome))
        {
            _logger.LogInformation("Atualizando nome do usuário ID {UserId} para {NewName} {NewLastName}", alterUser.userId, alterUser.nome ?? user.Name, alterUser.sobrenome ?? user.LastName);
            user.UpdateName(alterUser.nome ?? user.Name,
                            alterUser.sobrenome ?? user.LastName);
        } 

        if(alterUser.isAdmin != null)
        {
            _logger.LogInformation("Atualizando status de admin do usuário ID {UserId} para {IsAdmin}", alterUser.userId, alterUser.isAdmin);
            user.AlterAdmin(alterUser.isAdmin);
        }
            
        _logger.LogInformation("Salvando alterações do usuário ID {UserId}", alterUser.userId);

        await _commands.UpdateUser(user);

        _logger.LogInformation("Processo de atualização do usuário ID {UserId} concluído com sucesso", alterUser.userId);
    }
}
