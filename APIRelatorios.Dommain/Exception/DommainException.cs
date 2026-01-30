namespace APIRelatorios.Infra.Exeptions;

public class DommainException : Exception
{
    public DommainException(string message, Exception? inner = null)
        : base(message, inner) { }
}
