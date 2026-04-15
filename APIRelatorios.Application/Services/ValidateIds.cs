using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Services;

public class ValidateIds : IValidateIds
{
    private readonly IUserQuery _userQuery;
    private readonly IRotaQuery _rotaQuery;
    private readonly IEvidenciaRotaQuery _evidenciaQuery;

    public ValidateIds(IEvidenciaRotaQuery evidenciaQuery, IRotaQuery rotaQuery, IUserQuery userQuery)
    {
        _evidenciaQuery = evidenciaQuery;
        _rotaQuery = rotaQuery;
        _userQuery = userQuery;
    }

    public async Task<bool> UserExisteAsync(int id)
    {
        var fiscal  = await _userQuery.BuscarFiscalId(id);

        if (fiscal != null)
            return true;

        return false;
    }

    public async Task<bool> RotaExisteAsync(Guid id)
    {
        var rota = await _rotaQuery.BuscarRotaID(id);

        if (rota == null)
            return false;


        return true;
    }


    public async Task<bool> EvidenciaExisteAsync(Guid id)
    { 
        var evicencia = await _evidenciaQuery.GetEvidenciaId(id);

        if (evicencia != null)
            return true;

        return false;
    } 
}
