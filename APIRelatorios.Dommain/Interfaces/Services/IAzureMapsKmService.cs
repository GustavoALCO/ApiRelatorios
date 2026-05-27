namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IAzureMapsKmService
{
    Task<double?> BuscarDistanciaKM(List<(double, double)> coordenadas);
}
