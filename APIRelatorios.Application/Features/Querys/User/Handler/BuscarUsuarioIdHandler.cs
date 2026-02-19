using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.User.Handler;

public class BuscarUsuarioIdHandler
{
    public readonly IUserQuery _query;

    public BuscarUsuarioIdHandler(IUserQuery query)
    {
        _query = query;
    }

    public async Task<UsuarioDTO> Handler(BuscarUsuarioIdCommands commands)
    {
        var usuario = await _query.BuscarListaFiscalId(commands.UserId) ?? throw new Exception("Não há usuarios para ser listado");

        return new UsuarioDTO(
            usuario.UserId,
            usuario.Login,
            usuario.IsAdmin
        );
    }

}
