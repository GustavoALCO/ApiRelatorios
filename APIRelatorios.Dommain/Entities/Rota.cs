using System.Text.Json.Serialization;

namespace APIRelatorios.Dommain.Entities;

public class Rota
{
    public int RotaId { get; private set; }

    public string? NomeRota { get; private set; }

    public string Alimentador { get; private set; }

    public DateTime DataInicio { get; set; }

    public DateTime? DataFinal {  get; set; }

    public ICollection<EvidenciaRota> Images { get; set; } 

    public ICollection<UsuarioRota> Fiscais { get; set; } 

    public Rota()
    {
        
    }
    public Rota(string nomeRota,string alimentador ,DateTime dataInicio)
    {
        NomeRota = nomeRota;
        Alimentador = alimentador;
        DataInicio = dataInicio;
        Fiscais = new List<UsuarioRota>();
    }
    public void AdicionarFiscal(int userId)
    {
        Fiscais.Add(new UsuarioRota
        {
            UserId = userId
        });
    }
    public void AlterarNomeRota(string nomeRota)
    {
        NomeRota = nomeRota;    
    }
}
