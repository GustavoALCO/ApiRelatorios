using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct FinalizarRotaCommand
(
    Guid RotaId
) : ICommand;
