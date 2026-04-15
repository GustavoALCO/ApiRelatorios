using APIRelatorios.Dommain.Enuns;
using System.Text.Json.Serialization;

namespace APIRelatorios.Dommain.Entities;

public class Rota
{
    public Guid RotaId { get; private set; }

    public string? NomeRota { get; private set; }

    public string Alimentador { get; private set; }

    public Concessionarias Concessionarias { get; private set; }

    public double? Km {  get; private set; }

    public DateTime DataInicio { get; set; }

    public DateTime? DataFinal {  get; set; }

    public ICollection<EvidenciaRota> Images { get; set; } 

    public ICollection<UsuarioRota> Fiscais { get; set; } 

    public Rota()
    {
        
    }
    public Rota(Guid rotaId, string nomeRota,Concessionarias concessionarias ,string alimentador ,DateTime dataInicio)
    {
        RotaId = rotaId;
        NomeRota = nomeRota;
        Concessionarias = concessionarias;
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

    public void finalizandoRota(DateTime dataFinal, double km)
    {
        DataFinal = dataFinal;
        Km = km;
    }
}
