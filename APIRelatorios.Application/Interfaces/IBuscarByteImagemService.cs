namespace APIRelatorios.Application.Interfaces;

public interface IBuscarByteImagemService
{
    Task<byte[]> BaixarImagemAsync(string imageUrl);
}
