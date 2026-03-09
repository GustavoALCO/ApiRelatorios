using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Contracts.DTOs;

public class EvidenciaDTO
{
    public int EvidenciaRotaId { get;  set; }

    public int RotaId { get; set; }

    public string NomeFiscal { get;  set; }

    public int TemaFiscalizacao { get;  set; }

    public string? Alimentador { get;  set; }

    public string? Identificacao { get;  set; }

    public string? Descricao { get;  set; }

    public string ImageURL { get;  set; }

    public string Endereco { get;  set; }

    public string Cep {  get; set; }

    public string Horario { get;  set; }
}
