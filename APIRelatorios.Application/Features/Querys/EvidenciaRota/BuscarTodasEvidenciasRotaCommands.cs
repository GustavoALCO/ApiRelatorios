namespace APIRelatorios.Application.Features.Querys.EvidenciaRota;

public class BuscarTodasEvidenciasRotaCommands
{
    public Guid IdRota { get; set; }

    public int PageSize { get; set; }

    public int Page { get; set; }
}
