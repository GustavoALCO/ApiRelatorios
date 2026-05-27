using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Features.Queries.User;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.User.Handler;

public class BuscarTodosUsuariosHandler
    : IQueryHandler<BuscarTodosusuariosQuery, ICollection<UsuarioDTO>>
{
    private readonly IUserQuery _query;

    public BuscarTodosUsuariosHandler(IUserQuery query)
    {
        _query = query;
    }

    public async Task<ICollection<UsuarioDTO>> Handle(BuscarTodosusuariosQuery query, CancellationToken cancellationToken)
    {
        var fiscais = await _query.BuscarTodosFiscais() ?? throw new Exception("Não Existe Fiscais Cadastrados");

        ICollection<UsuarioDTO> fiscaisDTO = [];

        foreach(var fiscal in fiscais)
        {
            var dto = new UsuarioDTO
            {
                Name = $"{fiscal.Name} {fiscal.LastName}",
                UserId = fiscal.UserId
            };

            fiscaisDTO.Add(dto);
        }
        
        return fiscaisDTO;
    }
}
