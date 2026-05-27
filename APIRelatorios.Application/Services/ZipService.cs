using APIRelatorios.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace APIRelatorios.Application.Services;

public class ZipService : IZipService
{
    private readonly ILogger<ZipService> _logger;

    public ZipService(ILogger<ZipService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> CreateZipWithImagesAsync(
        byte[] docxBytes,
        List<(Func<Task<Stream>> StreamFactory, string Nome)> images)
    {
        using var memoryStream = new MemoryStream();

        _logger.LogInformation("Começando Processo de ZIP");

        using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            // 📄 DOCX
            var docEntry = zip.CreateEntry("relatorio.docx");

            using (var entryStream = docEntry.Open())
            using (var fileStream = new MemoryStream(docxBytes))
            {
                await fileStream.CopyToAsync(entryStream);
            }

            int index = 1;

            foreach (var image in images)
            {
                // limpa caracteres inválidos + espaços
                var nomeLimpo = string.Join("_",
                    image.Nome.Split(Path.GetInvalidFileNameChars()))
                    .Replace(" ", "_");

                var fileName = $"imagens/{index}_{nomeLimpo}.jpg";

                _logger.LogInformation($"Gravando imagem: {fileName}");

                var entry = zip.CreateEntry(fileName);

                using var entryStream = entry.Open();

                // 🔥 aqui está o streaming real
                using var imageStream = await image.StreamFactory();

                await imageStream.CopyToAsync(entryStream);

                index++;
            }
        }

        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }
}