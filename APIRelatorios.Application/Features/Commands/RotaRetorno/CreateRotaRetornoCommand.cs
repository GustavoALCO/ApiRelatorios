using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.RotaRetorno;

public readonly record struct CreateRotaRetornoCommand
(
    Guid rotaIdOriginal,

    Guid rotaId,

    List<int> Fiscais
);
