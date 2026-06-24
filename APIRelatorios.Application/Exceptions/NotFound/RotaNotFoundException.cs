namespace APIRelatorios.Application.Exceptions.NotFound;

public sealed class RotaNotFoundException : NotFoundException
{
    public RotaNotFoundException(Guid id) 
        : base(
            ErrorCodes.RotaNotFound, 
            $"Fiscalização com ID '{id}' não foi encontrado.")
    {
    }
}
