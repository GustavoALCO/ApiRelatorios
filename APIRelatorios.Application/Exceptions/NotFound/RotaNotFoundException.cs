namespace APIRelatorios.Application.Exceptions.NotFound;

public sealed class RotaNotFoundException : NotFoundException
{
    public RotaNotFoundException() 
        : base(
            ErrorCodes.RotaNotFound, 
            $"Fiscalização não foi encontrado.")
    {
    }
}
