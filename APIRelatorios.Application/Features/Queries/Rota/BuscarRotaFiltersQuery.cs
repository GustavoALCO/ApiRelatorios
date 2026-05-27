using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Features.Querys.Rota;

public class BuscarRotaFiltersQuery
    : IQuery<ICollection<RotaDTO>>
{
    public int FiscalId { get; set; }
    public string? Nome { get; set; }
    public string? DataInicial { get; set; }
    public string? DataFinal { get; set; }
    public int page { get; set; }
    public int pagesize {  get; set; }
}
