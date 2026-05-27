using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.RotaRetorno;

public readonly record struct CreateRotaRetornoCommand
(
    Guid rotaIdOriginal,

    Guid rotaId,

    List<int> Fiscais
) : ICommand;
