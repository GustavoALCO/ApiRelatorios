using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Domain.Entities;

public class Amostra
{
    //Id somente para o banco de dados
    public int Id { get; private set; }

    public Guid RotaId { get; private set; }

    //Atributos só para a visualização do app 
    public string SeqISA { get; private set; }
    public string SeqBaseFisica { get; private set; }
    public string? VlrBase { get; private set; }
    public string DescricaoTUC { get; private set; }
    public string DescricaoTec { get; private set; }
    public string ODIEngenharia { get; private set; }
    public string Instalacao { get; private set; }
    public string? Endereco { get; private set; }
    public string? Municipio { get;  private set; }
    public double? Latitude { get;  private set; }
    public double? Longitude { get;  private set; }

    //Atributos para o envio do app atraves de um boleano para confirmar atributos
    public double? LatitudeUser { get; private set; }
    public double? LongitudeUser { get; private set; }
    public string TUC1 { get;  private set; }
    public string? TUC2 { get;  private set; }
    public string? TUC3 { get;  private set; }
    public string? TUC4 { get;  private set; }
    public string? TUC5 { get;  private set; }
    public string? TUC6 { get; private set; }
    public string? NumSerie { get; private set; }
    public string? PosicaoOperativa { get; private set; }
    public string? Equipamento { get;  private set; }

    //Atributos para o envio do app em formato de texto
    public string? DataFabricacao { get;  private set; }
    public string? Observacao { get;  private set; }

    // Evidencia em Imagem
    public List<string>? Fotos { get;  private set; } = new();

    public Rota Rota { get;  set; }

    public Amostra()
    {

    }

    // Método para criar uma nova instância de Amostra com os atributos fornecidos
    public void createEquipamentoAmostra
        (
            Guid RotaId,
            string seqISA,
            string seqBaseFisica,
            string? vlrBase,
            string descricaoTUC,
            string descricaoTec,
            string odiEngenharia,
            string instalacao,
            string? endereco,
            string? municipio,
            double latitude,
            double longitude,
            string tuc1,
            string tuc2,
            string tuc3,
            string tuc4,
            string tuc5,
            string tuc6,
            string numSerie,
            string posicaoOperativa,
            string equipamento
        )
    {
        this.RotaId = RotaId;
        this.SeqISA = seqISA;
        this.SeqBaseFisica = seqBaseFisica;
        this.VlrBase = vlrBase;
        this.DescricaoTUC = descricaoTUC;
        this.DescricaoTec = descricaoTec;
        this.ODIEngenharia = odiEngenharia;
        this.Instalacao = instalacao;
        this.Endereco = endereco;
        this.Municipio = municipio;
        this.Latitude = latitude;
        this.Longitude = longitude;
        this.TUC1 = tuc1;
        this.TUC2 = tuc2;
        this.TUC3 = tuc3;
        this.TUC4 = tuc4;
        this.TUC5 = tuc5;
        this.TUC6 = tuc6;
        this.NumSerie = numSerie;
        this.PosicaoOperativa = posicaoOperativa;
        this.Equipamento = equipamento;
    }

    public void AtualizarEquipamentosAmostra
        (
            bool tuc1,
            bool? tuc2,
            bool? tuc3,
            bool? tuc4,
            bool? tuc5,
            bool? tuc6,
            bool? posicaoOperativa,
            bool? equipamento,
            bool? numSerie,
            string dataFabricacao,
            string observacao,
            List<string> fotos,
            double? latitudeUser,
            double? longitudeUser
        )
    {
        if(tuc1 == false)
            this.TUC1 = "Não Conforme";
        if(tuc2 == false)
            this.TUC2 = "Não Conforme";
        if (tuc3 == false)
            this.TUC3 = "Não Conforme";
        if (tuc4 == false)
            this.TUC4 = "Não Conforme";
        if (tuc5 == false)
            this.TUC5 = "Não Conforme";
        if (tuc6 == false)
            this.TUC6 = "Não Conforme";
        if (posicaoOperativa == false)
            this.PosicaoOperativa = "Não Conforme";
        if (equipamento == false)
            this.Equipamento = "Não Conforme";
        if (numSerie == false)
            this.NumSerie = "Não Conforme";
        if(!string.IsNullOrEmpty(dataFabricacao)) 
            this.DataFabricacao = dataFabricacao;
        if(!string.IsNullOrEmpty(observacao))
            this.Observacao = observacao;
        if (fotos?.Any() == true)
        {
            Fotos = (Fotos ?? new List<string>())
            .Concat(fotos)
            .ToList();
        }
        if (latitudeUser.HasValue && longitudeUser.HasValue)
        {
            this.LatitudeUser = latitudeUser;
            this.LongitudeUser = longitudeUser;
        }
    }
}
