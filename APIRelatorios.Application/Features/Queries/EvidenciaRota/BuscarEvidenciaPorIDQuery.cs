using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota;

public class BuscarEvidenciaPorIDQuery
:  IQuery<EvidenciaDTO>
{
    public Guid IdEvidencia {  get; set; }
} 
