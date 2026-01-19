namespace APIRelatorios.Dommain.Entities;

public class Rota
{
    public int RotaId { get; set; }

    public string? NomeRota { get; set; }

    public required DateTime DataInicio { get; set; }

    public DateTime? DataFinal {  get; set; }

    public ICollection<Imagem> Images { get; set; } = new List<Imagem>();

    public ICollection<UsuarioRota> Fiscais { get; set; } = new List<UsuarioRota>();

}
