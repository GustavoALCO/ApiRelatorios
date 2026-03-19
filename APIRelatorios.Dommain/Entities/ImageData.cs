namespace APIRelatorios.Dommain.Entities;

public class ImageData
{
    public int Id { get; set; }

    // Armazena a Imagem em Tamanho Real
    public string OriginalUrl { get; private set; }

    //Armazena a url da imagem em um tamanho medio
    public string MediumUrl { get; private set; }

    //Armazena na pior qualidade a imagem
    public string LowUrl { get; private set; }

    public Guid EvidenciaRotaId { get; set; } 
    public EvidenciaRota EvidenciaRota { get; set; }

    public ImageData()
    {
        
    }

    public ImageData(string urlOriginal, string urlMedium, string urlLow,Guid evidenciaId)
    {
        OriginalUrl = urlOriginal;
        MediumUrl = urlMedium;
        LowUrl = urlLow;
        EvidenciaRotaId = evidenciaId;
    }
}
