namespace APIRelatorios.Application.Exceptions.Integrations;

public sealed class DocxApiException : IntegrationException
{
    public DocxApiException() :
        base(
        ErrorCodes.DocxApiException,
        "O serviço de geração de relatórios está temporariamente indisponível. Tente novamente em alguns minutos."
        )
        {
    }
}
