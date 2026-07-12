namespace APIRelatorios.Application.Contracts.DTOs;

public class AmostraDTO
{
    //Id somente para o banco de dados
    public int Id { get;  set; }
    public Guid RotaId { get;  set; }
    //Atributos só para a visualização do app 
    public string SeqISA { get;  set; }
    public string SeqBaseFisica { get;  set; }
    public string? VlrBase { get;  set; }
    public string DescricaoTUC { get;  set; }
    public string DescricaoTec { get;  set; }
    public string ODIEngenharia { get;  set; }
    public string Instalacao { get;  set; }
    public string? Endereco { get;  set; }
    public string? Municipio { get;  set; }
    public double? Latitude { get;  set; }
    public double? Longitude { get;  set; }
    public string TUC1 { get;  set; }
    public string? TUC2 { get;  set; }
    public string? TUC3 { get;  set; }
    public string? TUC4 { get;  set; }
    public string? TUC5 { get;  set; }
    public string? TUC6 { get;  set; }
    public string? NumSerie { get;  set; }
    public string? PosicaoOperativa { get;  set; }
    public string? Equipamento { get;  set; }
    //Atributos para o envio do app em formato de texto
    public string DataFabricacao { get;  set; }
    public string Observacao { get;  set; }
    public List<string>? Fotos { get;  set; }
}
