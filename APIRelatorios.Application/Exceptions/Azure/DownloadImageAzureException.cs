namespace APIRelatorios.Application.Exceptions.Azure;

public sealed class DownloadImageAzureException : AzureErrorExceptions
{
    public DownloadImageAzureException() : 
        base(
        ErrorCodes.StorageError, 
        "Erro ao Baixar Imagens da Storage"
        )
    {
    }
}
