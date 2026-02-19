namespace APIRelatorios.Application.Features.Querys.Rota;

public class BuscarRotaFiltersCommands
{
    public int FiscalId { get; set; }
    public string? Nome { get; set; }
    public string? DataInicial { get; set; }
    public string? DataFinal { get; set; }
    public int page { get; set; }
    public int pagesize {  get; set; }
}
