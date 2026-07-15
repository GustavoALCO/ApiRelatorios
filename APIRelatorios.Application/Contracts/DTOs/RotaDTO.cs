using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Contracts.DTOs;

public readonly record struct RotaDTO
(
    Guid RotaId,
    string NomeRota,
    string Alimentador,
    string DataInicio,
    Concessionarias Concessionarias,
    string? DataFinal,
    TipoFiscalizacao TipoFiscalizacao
);

