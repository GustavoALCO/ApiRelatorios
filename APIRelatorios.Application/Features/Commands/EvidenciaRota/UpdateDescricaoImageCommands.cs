namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct UpdateDescricaoImageCommands
(
    int idMensage,
    string descricao
);
