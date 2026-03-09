using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class UpdatePasswordHandler
{
    private readonly IPasswordHasher _passwordHasher;

    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    public UpdatePasswordHandler(IUserCommands commands, IPasswordHasher passwordHasher, IUserQuery query)
    {
        _commands = commands;
        _passwordHasher = passwordHasher;
        _query = query;
    }

    public async Task Handler(UpdatePasswordCommand updPassword)
    {
        var user = await _query.BuscarFiscalId(updPassword.idUser)
            ?? throw new Exception("Id invalido de Usuario");

        user.UpdatePassword(_passwordHasher.HashPassword(updPassword.Password, user.Salt));

        await _commands.UpdateUser(user);
    }
}
