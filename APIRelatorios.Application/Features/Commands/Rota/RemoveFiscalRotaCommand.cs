namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct RemoveFiscalRotaCommand
(
    Guid rotaId,
    ICollection<int> fiscaisId
);