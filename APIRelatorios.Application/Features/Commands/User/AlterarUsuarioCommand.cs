namespace APIRelatorios.Application.Features.Commands.User;

public readonly record struct AlterarUsuarioCommand
(
    int userId,
    string nome,
    string senha,
    bool isAdmin
);
