using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota;

public class BuscarTodasEvidenciasRotaQuery
    : IQuery<ICollection<EvidenciaDTO>>
{
    public Guid IdRota { get; set; }

    public int PageSize { get; set; }

    public int Page { get; set; }
}
