using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct UpdateEvidenciasCommands
(
    Guid evidenciaId,

    string? descricao,

    int temaFiscalizacao,

    List<int> subTemaFiscalizacao,

    string? alimentador,

    string? endereco,

    string? identificacao,

    bool? emergencial
);
