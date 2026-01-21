namespace APIRelatorios.Dommain.Entities;

public class Rota
{
    public int RotaId { get; set; }

    public string? NomeRota { get; set; }

    public required DateTime DataInicio { get; set; }

    public DateTime? DataFinal {  get; set; }

    public ICollection<Imagem> Images { get; set; } = new List<Imagem>();

    public ICollection<UsuarioRota> Fiscais { get; set; } = new List<UsuarioRota>();

    public void AdicionarFiscal(int userId)
    {
        if (Fiscais.Any(f => f.UserID == userId))
            return; // ou throw, depende da regra

        Fiscais.Add(new UsuarioRota
        {
            UserID = userId
        });
    }
    public void RemoverFiscal(int userId)
    {
        var fiscal = Fiscais.FirstOrDefault(f => f.UserID == userId);

        if (fiscal is null)
            return; // ou throw, conforme sua regra

        Fiscais.Remove(fiscal);
    }
}
