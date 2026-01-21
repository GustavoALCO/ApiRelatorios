using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class UpdateUsuarioHandler
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    public UpdateUsuarioHandler(IUserQuery query, IUserCommands commands)
    {
        _query = query;
        _commands = commands;
    }

    public async Task Handler(AlterarUsuarioCommand alterUser)
    {
        var user = await _query.BuscarListaFiscalId(alterUser.userId)
            ?? throw new Exception("Erro ao Encontrar Usuario");

        user.Nome = alterUser.nome ?? user.Nome;
        user.Senha = alterUser.senha ?? user.Senha;
        
        if(alterUser.isAdmin != user.IsAdmin)
            user.IsAdmin = alterUser.isAdmin ;

        await _commands.UpdateUser(user);
    }
}
