namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IBuscarDistanciaCordenadas
{
    Task<double?> BuscarDistanciaKM(List<(double, double)> coordenadas);
}
