using APIRelatorios.Domain.Enuns;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Contracts.DTOs;

public class EvidenciaDTO
{
    public Guid EvidenciaRotaId { get;  set; }

    public Guid RotaId { get; set; }

    public string NomeFiscal { get;  set; }

    public TemaCheck temaFiscalizacao { get;  set; }

    public List<SubTemaAlimentadores> subTemaFiscalizacao { get;  set; }

    public string? Alimentador { get;  set; }

    public string? Identificacao { get;  set; }

    public string? Descricao { get;  set; }

    public List<string> ImageURL { get;  set; }

    public List<string> LowImageUrl { get; set; }

    public string Endereco { get;  set; }

    public string Cidade { get; set; }

    public double Latitude { get; set; }


    public double Longitude { get; set; }

    public DateTime Horario { get;  set; }

    public NivelRisco NivelRisco { get;  set; }
}
