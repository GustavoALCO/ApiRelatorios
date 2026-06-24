namespace APIRelatorios.Application.Exceptions.NotFound;

public sealed class ListEvidenciaNotFoundException : NotFoundException
{
    public ListEvidenciaNotFoundException() : 
        base(ErrorCodes.EvidenciaNotFound, "Não há Evidencia a serem retornadas")
    {
    }
}
