namespace APIRelatorios.Application.Exceptions.Business;

public sealed class RotaFinalizadaException : BusinessException
{
    public RotaFinalizadaException() 
        : base(
            ErrorCodes.BusinessRuleError,
            "Fiscalização Finzalizada, não é possível realizar essa ação."
        )
    {
    }
}
