namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct  UpdateNomeRotaCommand
(
    int rotaId,
    string nomeRota
);
