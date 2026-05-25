using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct CreateRelatorioWordCommand
(
    List<Guid> Ids
) : ICommand<byte[]>;
