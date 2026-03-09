using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct UpdateDescricaoImageCommands
(
    int evidenciaId,
    string? descricao,
    TemaFiscalizacao? tema,
    string? alimentador,
    string? endereco,
    string? identificacao
);
