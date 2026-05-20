namespace APIRelatorios.Application.Features.Commands.User;

public readonly record struct AlterarUsuarioCommand
(
    int userId,
    string? login,
    string? nome,
    string? sobrenome,
    bool? isAdmin
);
