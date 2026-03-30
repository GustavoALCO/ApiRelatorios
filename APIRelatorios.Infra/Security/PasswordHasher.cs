using APIRelatorios.Dommain.Interfaces.Services;
using DocumentFormat.OpenXml.Math;
using System.Security.Cryptography;

namespace APIRelatorios.Infra.Security;

public class PasswordHasher : IPasswordHasher
{

    public byte[] GenerateHash(int size = 16)
    {
        //cria um array de bytes com o tamanho de 16bytes
        var salt = new byte[size];
        //preenche com numeros aleatorios.
        RandomNumberGenerator.Fill(salt);
        //retorna o salt para ser armazenada
        return (salt);
    }

    public string HashPassword(string password, byte[] salt)
    {
        // Gera um Hash para criptografar a senha 
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(32); 
            return Convert.ToBase64String(hash);
        }
    }

    public bool VerifyPassword(string password,string saltPassword, byte[] salt)
    {
        string requestPassword = HashPassword(password, salt);

       return CryptographicOperations.FixedTimeEquals(
                     Convert.FromBase64String(requestPassword),
                     Convert.FromBase64String(saltPassword)
 );

    }
}
