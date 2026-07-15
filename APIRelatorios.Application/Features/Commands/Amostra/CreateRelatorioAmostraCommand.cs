using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Amostra;

public record struct CreateRelatorioAmostraCommand
(
    Guid idrota
): ICommand<byte[]>;