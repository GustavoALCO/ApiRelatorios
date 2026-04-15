using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Infra.Exeptions;
using System.Runtime.ConstrainedExecution;

namespace APIRelatorios.Dommain.Entities;

public class EvidenciaRota
{
    public Guid EvidenciaRotaId { get; private set; }

    public int FiscalId { get; private set; }

    public Guid RotaId { get; private set; }

    public Rota Rota { get; set; }

    public TemaFiscalizacao TemaFiscalizacao { get; private set; }

    public string? Alimentador { get; private set; }

    public string? Identificacão { get; private set; }

    public string? Descricao { get; private set; }

    public List<ImageData> Images { get; private set; } = new();

    public string Endereco { get; private set; }

    public string? Cidade { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public DateTime Horario { get; private set; }

    public bool Emergencial { get; private set; }

    public EvidenciaRota()
    {
        
    }

    public EvidenciaRota(
        Guid evidenciaRotaId,
        Guid rotaID,
        int fiscalId,
        TemaFiscalizacao tema,
        string? alimentador,
        string? identificacao,
        string? descricao,
        List<ImageData> imagem,
        string endereco,
        string cidade,
        double lat,
        double lon,
        DateTime horario,
        bool emergencial
        )
    {
        EvidenciaRotaId = evidenciaRotaId;
        RotaId = rotaID;
        FiscalId = fiscalId;
        TemaFiscalizacao = tema;    
        Alimentador = alimentador;
        Identificacão = identificacao;
        Descricao = descricao;
        Images = imagem ?? throw new DommainException("Erro Ao aplicar a url a entidade");
        Endereco = endereco;
        Cidade = cidade;
        Latitude = lat;
        Longitude = lon;
        Horario = horario;
        Emergencial = emergencial;
    }

    public void Atualizar(
     string? descricao,
     TemaFiscalizacao? tema,
     string? alimentador,
     string? endereco,
     string? identificacao)
    {
        if (descricao is not null)
            Descricao = descricao;

        if (tema.HasValue)
            TemaFiscalizacao = tema.Value;

        if (alimentador is not null)
            Alimentador = alimentador;

        if (endereco is not null)
            Endereco = endereco;

        if (identificacao is not null)
            Identificacão = identificacao;
    }
}
