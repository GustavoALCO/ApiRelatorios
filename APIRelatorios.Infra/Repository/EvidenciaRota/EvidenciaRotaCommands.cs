using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace APIRelatorios.Infra.Repository.Images;

public class EvidenciaRotaCommands : IEvidenciaRotaCommands
{

    private readonly DatabaseContext _databaseContext;

    public EvidenciaRotaCommands(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task DeleteImage(EvidenciaRota img)
    {
        try
        {
            _databaseContext.EvidenciaRota.Remove(img);

            await _databaseContext.SaveChangesAsync();
        }
        catch(DbUpdateException ex)
        {
            throw new RepositoryException("Não foi Possivel Remover Imagem do Banco de dados",
                ex);
        }
    }

    public async Task SaveImage(EvidenciaRota img)
    {
        try
        {
            await _databaseContext.EvidenciaRota.AddAsync(img);

            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("Não foi Possivel Adicionar Imagem do Banco de dados",
                ex);
        }
    }

    public async Task UpdateImageAsync(EvidenciaRota img)
    {
        try
        {
            _databaseContext.EvidenciaRota.Update(img);

            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("Não foi Possivel Adicionar Imagem do Banco de dados",
                ex);
        }
    }
}
