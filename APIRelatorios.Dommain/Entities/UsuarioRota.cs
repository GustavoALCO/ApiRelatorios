namespace APIRelatorios.Dommain.Entities;

public class UsuarioRota
{
    public int RotaID { get; set; }
    public Rota Rota { get; set; } = null!;

    public int UserID { get; set; }

    public User User { get; set; } = null!;
}
