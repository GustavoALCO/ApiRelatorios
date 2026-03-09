using System.Text.Json.Serialization;

namespace APIRelatorios.Dommain.Entities;

public class User
{
    public int UserId { get; private set; }

    public string Login { get; private set; }

    public string Name { get; private set; }

    public string LastName { get; private set; }

    public string HashPassword { get; private set; }

    public byte[] Salt { get; private set; }

    public bool IsAdmin { get; private set; }   

    public  bool IsValid { get; private set; }

    [JsonIgnore]
    public ICollection<UsuarioRota> usuarioRotas { get; set; } = new List<UsuarioRota>();

    public User()
    {
        
    }

    public void CreateUser(string login,string name,string lastname ,string hashPassword,byte[] salt, bool? isAdmin)
    {
        Login = login;
        Name = name;
        LastName = lastname;
        HashPassword = hashPassword;
        Salt = salt;
        IsAdmin = isAdmin ?? false;
        IsValid = true;
    }

    public void UpdatePassword(string password)
    {
        HashPassword = password;
    }

    public void UpdateName(string name, string lastname)
    {
        if (name != null) Name = name;
        if(lastname != null) LastName = lastname;
    }

    public void UpdateLogin(string login)
    {
        Login = login; 
    }

    public void AlterAdmin(bool? isadmin)
    {
        IsAdmin = !isadmin ?? throw new Exception("Erro ao não passar valor a usuario");
    }

    public void AlterValid(bool? isvalid)
    {
        IsValid = !isvalid ??throw new Exception("Erro ao não passar valor a usuario");
    }
}
