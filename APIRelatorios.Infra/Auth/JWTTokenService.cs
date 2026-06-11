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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName)
        };

        // Adiciona role se for admin
        if (isAdmin)
            claims.Add(new Claim("Role", "Admin"));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience[0], 
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_configuration.ExpireDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}