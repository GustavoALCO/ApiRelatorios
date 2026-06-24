namespace APIRelatorios.Application.Exceptions.NotFound;

public abstract class NotFoundException : AppException
{
    protected NotFoundException(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}