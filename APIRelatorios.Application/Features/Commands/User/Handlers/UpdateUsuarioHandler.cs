using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class UpdateUsuarioHandler
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    private readonly IValidateIds _validateIds;

    public UpdateUsuarioHandler(IUserQuery query, IUserCommands commands, IValidateIds validateIds)
    {
        _query = query;
        _commands = commands;
        _validateIds = validateIds;
    }

    public async Task Handler(AlterarUsuarioCommand alterUser)
    {
        if (await _validateIds.UserExisteAsync(alterUser.userId) is false)
            throw new Exception("Id invalido");

        var user = await _query.BuscarFiscalId(alterUser.userId)
            ?? throw new Exception("Erro ao Encontrar Usuario");

        if (!string.IsNullOrEmpty(alterUser.login))
            user.UpdateLogin(alterUser.login);

        if (!string.IsNullOrEmpty(alterUser.nome) || !string.IsNullOrEmpty(alterUser.sobrenome))
            user.UpdateName(alterUser.nome,
                            alterUser.sobrenome);

        if(alterUser.isAdmin != null)
            user.AlterAdmin(alterUser.isAdmin);

        await _commands.UpdateUser(user);
    }
}
