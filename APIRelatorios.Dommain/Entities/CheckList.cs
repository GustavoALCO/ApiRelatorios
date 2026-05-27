using System.Text.Json.Serialization;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Entities;

public class CheckList
{
    [JsonIgnore]
    public int Id { get; set; }

    public Guid EvidenciaRotaId { get; set; }

    public TemaCheck TemaCheck { get; set; }

    public List<SubTemaAlimentadores> SubTemaAlimentadores { get; set; }
        = new();


    [JsonIgnore]
    public EvidenciaRota? EvidenciaRota { get; set; }

    public CheckList()
    {
        
    }

    public CheckList(Guid evidenciaRotaId, TemaCheck temaCheck, List<SubTemaAlimentadores> subTemaAlimentadores)
    {
        EvidenciaRotaId = evidenciaRotaId;
        TemaCheck = temaCheck;
        SubTemaAlimentadores = subTemaAlimentadores;
    }
    
}