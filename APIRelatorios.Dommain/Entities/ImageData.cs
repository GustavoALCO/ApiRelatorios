namespace APIRelatorios.Dommain.Entities;

public class ImageData
{
    public int Id { get; set; }

    // Armazena a Imagem em Tamanho Real
    public string OriginalUrl { get; private set; }

    //Armazena na pior qualidade a imagem
    public string LowUrl { get; private set; }

    public Guid EvidenciaRotaId { get; set; } 
    
    public EvidenciaRota EvidenciaRota { get; set; }

    public ImageData()
    {
        
    }

    public ImageData(string urlOriginal, string urlLow,Guid evidenciaId)
    {
        OriginalUrl = urlOriginal;
        LowUrl = urlLow;
        EvidenciaRotaId = evidenciaId;
    }
}
