namespace APIRelatorios.Application.Exceptions.InternalError;

public class InternalErrorException : AppException
{
    protected InternalErrorException(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}
