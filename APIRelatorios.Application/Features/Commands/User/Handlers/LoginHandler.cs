using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.Authentication;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class LoginHandler
    : ICommandHandler<LoginCommandsCommand, string>
{
    private readonly IUserQuery _query;

    private readonly IJwtTokenService _jwtTokenService;

    private readonly IPasswordHasher _passwordHasher;

    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(IUserQuery query, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher, ILogger<LoginHandler> logger )
    {
        _query = query;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<string> Handle(LoginCommandsCommand commands, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando processo de login para o usuário {Login}", commands.Login);

        var usuario = await _query.BuscarFiscalLogin(commands.Login) 
            ?? throw new InvalidCredentialsException();

        var isTrue = _passwordHasher.VerifyPassword(commands.Senha, usuario.HashPassword, usuario.Salt);

        if (isTrue == false)
        {
            _logger.LogWarning("Falha no login para o usuário {Login}: senha incorreta", commands.Login);
            throw new InvalidCredentialsException();
        }

        _logger.LogInformation("Login bem-sucedido para o usuário {Login}", commands.Login);

        return _jwtTokenService.GenerateToken(usuario.UserId, usuario.Login, usuario.IsAdmin);
    }
}
