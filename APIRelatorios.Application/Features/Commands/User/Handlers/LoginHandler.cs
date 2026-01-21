using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class LoginHandler
{
    private readonly IUserQuery _query;

    private readonly IJwtTokenService _jwtTokenService;

    public LoginHandler(IUserQuery query, IJwtTokenService jwtTokenService)
    {
        _query = query;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handler(LoginCommands commands)
    {
        var usuario = await _query.BuscarFiscalNome(commands.Login) 
            ?? throw new Exception("Usuario não encontrado");

        if (usuario.Senha != commands.Senha)
            throw new Exception("Credenciais inválidas");

        return _jwtTokenService.GenerateToken(usuario.UserId, usuario.Nome, usuario.IsAdmin);
    }
}
