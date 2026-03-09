using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.User.Handler;

public class BuscarTodosUsuariosHandler
{
    private readonly IUserQuery _query;

    public BuscarTodosUsuariosHandler(IUserQuery query)
    {
        _query = query;
    }

    public async Task<ICollection<UsuarioDTO>> Handler()
    {
        var fiscais = await _query.BuscarTodosFiscais() ?? throw new Exception("Não Existe Fiscais");

        ICollection<UsuarioDTO> fiscaisDTO = [];

        foreach(var fiscal in fiscais)
        {
            var dto = new UsuarioDTO
            {
                IsAdmin = fiscal.IsAdmin,
                Login = fiscal.Login,
                UserId = fiscal.UserId
            };

            fiscaisDTO.Add(dto);
        }
        
        return fiscaisDTO;
    }
}
