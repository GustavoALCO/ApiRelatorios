using APIRelatorios.Application.Contracts.Enum;

namespace APIRelatorios.Application.Contracts.DTOs;

public  record struct EmergencialDTO
(
    string Conc,

    List<int> Dias,

    string Mes,

    string Ano,

    List<EvidenciaDocs> Evidencias
);
