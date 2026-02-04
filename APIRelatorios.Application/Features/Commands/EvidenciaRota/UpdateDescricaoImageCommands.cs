namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct UpdateDescricaoImageCommands
(
    int idDescricao,
    string descricao
);
