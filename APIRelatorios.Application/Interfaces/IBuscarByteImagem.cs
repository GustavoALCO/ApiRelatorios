namespace APIRelatorios.Application.Interfaces;

public interface IBuscarByteImagem
{
    Task<byte[]> BaixarImagemAsync(string imageUrl);
}
