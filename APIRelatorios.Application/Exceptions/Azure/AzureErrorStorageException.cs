namespace APIRelatorios.Application.Exceptions.Azure;

public sealed class AzureErrorStorageException : AzureErrorExceptions
{
    public AzureErrorStorageException()
        : base(
            ErrorCodes.StorageError,
            "Erro ao Enviar Imagem à Storage")
    {
    }
}
