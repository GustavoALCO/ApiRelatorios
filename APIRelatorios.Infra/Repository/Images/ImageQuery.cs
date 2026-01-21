using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace APIRelatorios.Infra.Repository.Images;

public class ImageQuery : IImageQuery
{
    private readonly DatabaseContext _Context;

    public ImageQuery(DatabaseContext context)
    {
        _Context = context;
    }

    public async Task<Imagem> GetImageId(int imageId)
    {
       
        var image = await _Context.Imagem.FirstOrDefaultAsync(i => i.ImagemId == imageId);

        return image;
        
    }

    async Task<ICollection<Imagem>> IImageQuery.GetImagemAsync(int RodaId)
    {
        try
        {
            var image = await _Context.Imagem.Where(i => i.RotaID == RodaId).ToListAsync();

            return image;
        }
        catch (InvalidOperationException ex)
        {
            throw new RepositoryException("Erro ao buscar informações da imagem no banco de dados.",
                ex);
        }
    }
}
