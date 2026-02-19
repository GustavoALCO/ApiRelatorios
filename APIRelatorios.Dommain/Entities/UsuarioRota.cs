namespace APIRelatorios.Dommain.Entities;

public class UsuarioRota
{
    public int RotaId { get; set; }
    public Rota Rota { get; set; } = null!;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
