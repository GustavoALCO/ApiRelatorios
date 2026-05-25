using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct AddFiscalRotaCommand
(
    Guid rotaId,
    ICollection<int> FiscaisId
    
) : ICommand;
