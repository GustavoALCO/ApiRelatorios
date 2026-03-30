namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct  UpdateNomeRotaCommand
(
    Guid rotaId,
    string nomeRota
);
