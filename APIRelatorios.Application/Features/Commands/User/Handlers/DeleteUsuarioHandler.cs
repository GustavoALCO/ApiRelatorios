using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class DeleteUsuarioHandler
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    private readonly IValidateIds _validateIds;

    public DeleteUsuarioHandler(IUserQuery query, IUserCommands commands, IValidateIds validateIds)
    {
        _validateIds = validateIds;
        _query = query;
        _commands = commands;
    }

    public async Task Handler(DeleteUsuarioCommand dltuser)
    {
        if (await _validateIds.UserExisteAsync(dltuser.idUser) is false)
            throw new Exception("Id invalido");

        var user = await _query.BuscarFiscalId(dltuser.idUser)
            ?? throw new Exception("Erro ao Encontrar Usuario");

        await _commands.DeleteUser(user);
    }
}
