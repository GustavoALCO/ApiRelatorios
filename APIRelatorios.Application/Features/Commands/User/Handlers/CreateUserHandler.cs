using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class CreateUserHandler
{
    private readonly IUserCommands _commands;

    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IUserQuery query, IUserCommands commands, IPasswordHasher passwordHasher)
    {
        _commands = commands;
        _passwordHasher = passwordHasher;
    }

    public async Task Handler(CreateUsuarioCommand createuser)
    {
        Dommain.Entities.User user = new();

        //Gera um salt para ser armazenado no banco de dados
        var salt = _passwordHasher.GenerateHash();

        var hashPassword = _passwordHasher.HashPassword(createuser.senha, salt);

        user.CreateUser(createuser.login, createuser.nome, createuser.sobreNome, hashPassword, salt, createuser.isAdmin);
        

        await _commands.CreateUser(user);
    }
}
