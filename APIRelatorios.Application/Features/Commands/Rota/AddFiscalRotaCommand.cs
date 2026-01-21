namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct AddFiscalRotaCommand
(
    int rotaId,
    ICollection<int> FiscaisId
);
