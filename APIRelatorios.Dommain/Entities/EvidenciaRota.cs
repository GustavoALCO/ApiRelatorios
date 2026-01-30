using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Infra.Exeptions;
using System.Runtime.ConstrainedExecution;

namespace APIRelatorios.Dommain.Entities;

public class EvidenciaRota
{
    public int EvidenciaRotaId { get; private set; }

    public int RotaID { get; private set; }
    public Rota Rota { get; set; }
    public TemaFiscalizacao TemaFiscalizacao { get; set; }

    public string Alimentador { get; private set; }

    public string? Descricao { get; private set; }

    public string ImageURL { get; private set; }

    public string Endereco { get; private set; }

    public string Cep { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public DateTime Horario { get; private set; }



    public EvidenciaRota()
    {
        
    }

    public EvidenciaRota(int rotaID,
        TemaFiscalizacao tema,
        string alimentador,
        string descricao,
        string imagem,
        string endereco,
        string cep,
        double lat,
        double lon)
    {
        RotaID = rotaID;
        TemaFiscalizacao = tema;    
        Alimentador = alimentador;
        Descricao = descricao;
        ImageURL = imagem ?? throw new DommainException("Erro Ao aplicar a url a entidade");
        Endereco = endereco;
        Cep = cep;
        Latitude = lat;
        Longitude = lon;
        Horario = DateTime.UtcNow;
    }

    public void AlterarDescricao(string descricao)
    {
        Descricao = descricao;
    }
}
