namespace APIRelatorios.Application.Interfaces;

public interface IZipService
{
    Task<byte[]> CreateZipWithImagesAsync(
        byte[] docxBytes,
        List<(Func<Task<Stream>> StreamFactory, string Nome)> images);
}
