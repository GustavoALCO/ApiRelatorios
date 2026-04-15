using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Entities;

public class RotaRetorno : Rota
{

    public RotaRetorno()
    {
        
    }

    public RotaRetorno(Guid rotaId, string nomeRota, Concessionarias concessionarias, string alimentador, DateTime dataInicio)
        :base (rotaId, nomeRota, concessionarias, alimentador, dataInicio)
    {
    
    }
}
