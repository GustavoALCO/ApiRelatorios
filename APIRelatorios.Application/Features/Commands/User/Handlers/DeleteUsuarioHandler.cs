using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class DeleteUsuarioHandler
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    public DeleteUsuarioHandler(IUserQuery query, IUserCommands commands)
    {
        _query = query;
        _commands = commands;
    }

    public async Task Handler(DeleteUsuarioCommand dltuser)
    {
        var user = await _query.BuscarListaFiscalId(dltuser.idUser)
            ?? throw new Exception("Erro ao Encontrar Usuario");

        await _commands.DeleteUser(user);
    }
}
