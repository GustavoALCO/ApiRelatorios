namespace APIRelatorios.Domain.Interfaces.Services;

public interface IAzureMapsEnderecoService
{
    Task<(string, string)?> BuscarNomeRua(double lat, double lng);
}
