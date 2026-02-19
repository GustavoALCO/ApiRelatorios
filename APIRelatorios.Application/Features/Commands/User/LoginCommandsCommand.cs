namespace APIRelatorios.Application.Features.Commands.User;

public record struct LoginCommandsCommand
(
    string Login,
    string Senha
);