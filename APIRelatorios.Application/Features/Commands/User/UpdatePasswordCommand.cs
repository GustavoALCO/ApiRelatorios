using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.User;

public readonly record struct UpdatePasswordCommand
(
    int idUser,
    string Password
) : ICommand;

