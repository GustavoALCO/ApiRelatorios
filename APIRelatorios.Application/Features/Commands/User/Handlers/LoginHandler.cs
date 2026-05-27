using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class LoginHandler
    : ICommandHandler<LoginCommandsCommand, string>
{
    private readonly IUserQuery _query;

    private readonly IJwtTokenService _jwtTokenService;

    private readonly IPasswordHasher _passwordHasher;

    public LoginHandler(IUserQuery query, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher)
    {
        _query = query;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> Handle(LoginCommandsCommand commands, CancellationToken cancellationToken)
    {
        var usuario = await _query.BuscarFiscalLogin(commands.Login) 
            ?? throw new Exception("Verifique se o login ou a senha está certa");

        var isTrue = _passwordHasher.VerifyPassword(commands.Senha, usuario.HashPassword, usuario.Salt);

        if (isTrue == false)
            throw new Exception("Verifique se o login ou a senha está certa");

        return _jwtTokenService.GenerateToken(usuario.UserId, usuario.Login, usuario.IsAdmin);
    }
}
