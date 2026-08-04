using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Domain.Entities;

namespace APIRelatorios.Application.Features.Queries.Amostra;

public record struct BuscarJsonAmostrasColetadasQuery
(
    Guid idrota
) : IQuery<IEnumerable<AmostraDTO>>;    
    