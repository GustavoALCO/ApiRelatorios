using APIRelatorios.Application.Settings;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIRelatorios.Infra.Auth;

public class JWTTokenService : IJwtTokenService
{
    private readonly JWTSettings _configuration;

    public JWTTokenService(IOptions<JWTSettings> jwtSettings)
    {
        _configuration = jwtSettings.Value;
    }

    public string GenerateToken(int userId, string userName, bool isAdmin)
    {
        var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var credentials = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("Id", userId.ToString()),
            new Claim("Username", userName),
            new Claim("Admin", isAdmin.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience[0],
            claims: claims,
            expires: DateTime.Now.AddDays(_configuration.ExpireDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
