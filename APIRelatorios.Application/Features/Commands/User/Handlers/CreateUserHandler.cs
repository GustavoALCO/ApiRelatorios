using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class CreateUserHandler
{
    private readonly IUserCommands _commands;

    public CreateUserHandler(IUserQuery query, IUserCommands commands)
    {
        _commands = commands;
    }

    public async Task Handler(CreateUsuarioCommand createuser)
    {
        Dommain.Entities.User user = new()
        { 
            Nome = createuser.nome,
            Senha = createuser.senha,
            IsAdmin = createuser.isAdmin,
            IsActive = true
        };

        await _commands.CreateUser(user);
    }
}
