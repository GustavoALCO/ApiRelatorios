using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace APIRelatorios.Infra.Repository.Images;

public class ImageCommands : IImageCommands
{

    private readonly DatabaseContext _databaseContext;

    public ImageCommands(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task DeleteImage(Imagem img)
    {
        try
        {
            _databaseContext.Imagem.Remove(img);

            await _databaseContext.SaveChangesAsync();
        }
        catch(DbUpdateException ex)
        {
            throw new RepositoryException("Não foi Possivel Remover Imagem do Banco de dados",
                ex);
        }
    }

    public async Task SaveImage(Imagem img)
    {
        try
        {
            await _databaseContext.Imagem.AddAsync(img);

            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("Não foi Possivel Adicionar Imagem do Banco de dados",
                ex);
        }
    }

    public async Task UpdateImageAsync(Imagem img)
    {
        try
        {
            _databaseContext.Imagem.Update(img);

            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("Não foi Possivel Adicionar Imagem do Banco de dados",
                ex);
        }
    }
}
