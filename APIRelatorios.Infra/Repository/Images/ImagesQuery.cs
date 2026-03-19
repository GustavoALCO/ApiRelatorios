using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Infra.Repository.Images;

public class ImagesQuery : IImagesQuery
{
    private readonly DatabaseContext _context;

    private readonly ILogger<ImagesQuery> logger;

    public ImagesQuery(DatabaseContext context, ILogger<ImagesQuery> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<List<ImageData>> GetImageEvidencia(Guid evidenciaRotaId)
    {

        var image = await _context.Images.Where(x => x.EvidenciaRotaId == evidenciaRotaId).ToListAsync();

        logger.LogInformation($"{image.Count()}");

        return image;

    }
}
