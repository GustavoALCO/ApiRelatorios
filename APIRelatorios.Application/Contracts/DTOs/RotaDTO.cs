using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Application.Contracts.DTOs;

public readonly record struct RotaDTO
(
    int RotaId,
    string NomeRota,
    string Alimentador,
    string DataInicio,
    string? DataFinal
);

