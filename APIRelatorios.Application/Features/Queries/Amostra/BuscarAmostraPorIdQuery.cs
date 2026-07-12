using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Features.Queries.Amostra;

public record struct BuscarAmostraPorIdQuery
(
    int id
) : IQuery<AmostraDTO>;