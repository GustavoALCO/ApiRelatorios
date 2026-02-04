using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Application.Contracts.DTOs;

public readonly record struct RotaDTO
(
    int RotaId,
    string NomeRota,
    DateTime DataInicio,
    DateTime? DataFinal
);

