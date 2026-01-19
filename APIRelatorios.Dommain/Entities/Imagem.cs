namespace APIRelatorios.Dommain.Entities;

public class Imagem
{
    public int ImagemId { get; set; }

    public int RotaID { get; set; }

    public string? Descricao { get; set; }

    public required string ImageURL { get; set; }

    public required string Endereco { get; set; }

    public required int Cep { get; set; }

    public required double Latitude { get; set; }

    public required double Longitude { get; set; }

    public required DateTime Horario { get; set; }

    public Rota Rota { get; set; }
}
