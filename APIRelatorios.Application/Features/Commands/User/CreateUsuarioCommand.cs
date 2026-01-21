namespace APIRelatorios.Application.Features.Commands.User;

public readonly record struct CreateUsuarioCommand
(
    string nome,
    string senha,
    bool isAdmin
);
