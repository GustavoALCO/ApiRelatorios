namespace APIRelatorios.Application.Exceptions.Azure;

public sealed class AzureErrorMapsException : AzureErrorExceptions
{
    public AzureErrorMapsException()
        : base(
            ErrorCodes.MapsError,
            "Erro ao Carregar Rota Percorrida")
    {
    }
}
