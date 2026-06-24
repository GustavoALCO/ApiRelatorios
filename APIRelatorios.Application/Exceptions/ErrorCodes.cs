namespace APIRelatorios.Application.Exceptions;

public static class ErrorCodes
{
    public const string UserNotFound = "USER_NOT_FOUND";

    public const string RotaNotFound = "FISCALIZACAO_NOT_FOUND";

    public const string EvidenciaNotFound = "EVIDENCIA_NOT_FOUND";

    public const string InvalidCredentials = "INVALID_CREDENTIALS";

    public const string StorageError = "STORAGE_ERROR";

    public const string MapsError = "MAPS_ERROR";

    public const string Base64Error = "BASE64_ERROR";

    public const string ValidationError = "VALIDATION_ERROR";

    public const string BusinessRuleError = "BUSINESS_RULE_ERROR";

    public const string DocxApiException = "DOCX_API_EXCEPTION";

    public const string InternalError = "INTERNAL_ERROR";
}