using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Rota;
public readonly record struct DeleteRotaCommand
(
    Guid RotaId
) : ICommand;
