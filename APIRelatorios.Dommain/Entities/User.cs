namespace APIRelatorios.Dommain.Entities;

public class User
{
    public int UserId { get; set; }

    public required string Nome { get; set; }

    public required string Senha { get; set; }

    public required bool IsAdmin { get; set; }   

    public required bool IsActive { get; set; }

    public ICollection<UsuarioRota> usuarioRotas { get; set; } = new List<UsuarioRota>();
}
