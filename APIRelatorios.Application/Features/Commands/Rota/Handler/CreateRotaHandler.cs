using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRotaHandler
    : ICommandHandler<CreateRotaCommand>
{

    private readonly IRotaCommands _rotaCommands;

    private readonly IValidateIds _validateIds;

    public CreateRotaHandler(IRotaCommands rotaCommands, IUserQuery userQuery, IValidateIds validateIds)
    {
        _rotaCommands = rotaCommands;
        _validateIds = validateIds;
    }

    public async Task Handle(CreateRotaCommand command, CancellationToken cancellationToken)
    {
        foreach(var fiscais in command.Fiscais)
        {
            if (await _validateIds.UserExisteAsync(fiscais) is false)
                throw new ListUsersNotFoundException();
        }

        Dommain.Entities.Rota rota = new(
                                        command.rotaId ?? Guid.NewGuid(),
                                        command.NomeRota,
                                        command.Concessionarias,
                                        command.Alimentador,
                                         DateTime.UtcNow);

        foreach (var userId in command.Fiscais)
        {
            UsuarioRota usuarioRota = new()
            {
            UserId = userId
            };

            rota.Fiscais.Add(usuarioRota);
        }

        rota.DataInicio.AddHours(-3);

        await _rotaCommands.CreateRotaAsync(rota);
    }
}
