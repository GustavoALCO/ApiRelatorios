namespace APIRelatorios.Domain.Interfaces.Amostra;

public interface IAmostraCommands
{
    Task SaveAmostra(Domain.Entities.Amostra amostra);

    Task UpdateAmostra(Domain.Entities.Amostra amostra);
}
