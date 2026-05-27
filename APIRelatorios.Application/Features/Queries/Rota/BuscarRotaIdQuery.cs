using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Features.Queries.Rota;

public class BuscarRotaIdQuery
: IQuery<RotaDTO>
{
    public Guid RotaId { get; set; } 
}
