namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IPasswordHasher
{
    public byte[] GenerateHash(int size = 16);

    public string HashPassword(string password, byte[] salt);

    public bool VerifyPassword(string password, string saltPassword, byte[] salt);
}
