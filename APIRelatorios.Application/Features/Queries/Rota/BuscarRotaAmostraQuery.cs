using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Features.Queries.Rota;

public readonly record struct BuscarRotaAmostraQuery
(
    int idUser
) : IQuery<ICollection<RotaDTO>>;