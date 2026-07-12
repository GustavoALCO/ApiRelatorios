namespace APIRelatorios.Application.Exceptions.NotFound;

public class AmostraNotFoundException : NotFoundException
{
    public AmostraNotFoundException() : base(
        errorCode: ErrorCodes.AmostraNotFound,
        message: "Amostra não encontrada."
    )
    {
    }
}
