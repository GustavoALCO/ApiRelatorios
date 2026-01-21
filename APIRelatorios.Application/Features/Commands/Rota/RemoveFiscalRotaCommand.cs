namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct RemoveFiscalRotaCommand
(
    int rotaId,
    ICollection<int> fiscaisId
);