namespace APIRelatorios.Application.Exceptions.NotFound;

public sealed class EvidenciaNotFoundException : NotFoundException
{
    public EvidenciaNotFoundException(Guid id)
        : base(
            ErrorCodes.UserNotFound,
            $"Evidencia com ID '{id}' não foi encontrada."
        )
    {
    }
}
