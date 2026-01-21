namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(int userId, string userName, bool isAdmin);
}
