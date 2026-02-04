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

        var user = await _query.BuscarListaFiscalId(alterUser.userId)
            ?? throw new Exception("Erro ao Encontrar Usuario");

        user.Nome = alterUser.nome ?? user.Nome;
        user.Senha = alterUser.senha ?? user.Senha;
        
        if(alterUser.isAdmin != user.IsAdmin)
            user.IsAdmin = alterUser.isAdmin ;

        await _commands.UpdateUser(user);
    }
}
