using APIRelatorios.Domain.Enuns;

namespace APIRelatorios.Dommain.Entities;

public class EvidenciaRota
{
    public Guid EvidenciaRotaId { get; private set; }

    public int FiscalId { get; private set; }

    public Guid RotaId { get; private set; }

    public Rota Rota { get; set; }

    public CheckList CheckList { get; private set; }

    public string? Alimentador { get; private set; }

    public string? Identificacão { get; private set; }

    public string? Descricao { get; private set; }

    public List<ImageData> Images { get; private set; } = new();

    public string Endereco { get; private set; }

    public string Cidade { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public DateTime Horario { get; private set; }

    public NivelRisco NivelRisco { get; private set; }

    public bool IsValid {get; private set;}

    public EvidenciaRota()
    {
        
    }

    public EvidenciaRota(
        Guid evidenciaRotaId,
        Guid rotaID,
        int fiscalId,
        CheckList checkList,
        string? alimentador,
        string? identificacao,
        string? descricao,
        List<ImageData> imagem,
        string endereco,
        string cidade,
        double lat,
        double lon,
        DateTime horario,
        NivelRisco emergencial
        )
    {
        EvidenciaRotaId = evidenciaRotaId;
        RotaId = rotaID;
        FiscalId = fiscalId;
        CheckList = checkList;    
        Alimentador = alimentador;
        Identificacão = identificacao;
        Descricao = descricao;
        Images = imagem ?? throw new ArgumentNullException("Erro Ao aplicar a url a entidade");
        Endereco = endereco;
        Cidade = cidade;
        Latitude = lat;
        Longitude = lon;
        Horario = horario;
        NivelRisco = emergencial;
        IsValid = true;
    }

    public void Atualizar(
     string? descricao,
     CheckList? tema,
     string? alimentador,
     string? endereco,
     string? identificacao,
     NivelRisco? emergencial
     )
    {
        if (tema is not null)
            CheckList = tema;

        if (descricao is not null)
            Descricao = descricao;

        if (alimentador is not null)
            Alimentador = alimentador;

        if (endereco is not null)
            Endereco = endereco;

        if (identificacao is not null)
            Identificacão = identificacao;

        if (emergencial is not null)
            NivelRisco = (NivelRisco)emergencial;

    }

    public void DesativarEvidencia()
    {
        IsValid = false;
    }
}
