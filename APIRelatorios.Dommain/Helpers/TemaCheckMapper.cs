using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;
public static class TemaCheckMapper
{
    private static readonly Dictionary<TemaCheck, string> _map =
        new()
        {
            { TemaCheck.SaidaDoAlimentador, "Saída do Alimentador" },
            { TemaCheck.Vegetacao, "Vegetação" },
            { TemaCheck.Postes, "Postes" },
            { TemaCheck.Cruzetas, "Cruzetas" },
            { TemaCheck.Isoladores, "Isoladores" },
            { TemaCheck.Condutores, "Condutores" },
            { TemaCheck.Seguranca, "Segurança" },
            { TemaCheck.Aterramento, "Aterramento" },
            { TemaCheck.Transformadores, "Transformadores" },
            { TemaCheck.ChavesReligadores, "Chaves/Religadores" },
            { TemaCheck.ParaRaios, "Para-raios" },
            { TemaCheck.EquipamentoInativo, "Equipamento Inativo" },
            { TemaCheck.EquipamentoSemIdentificacao, "Equipamento sem Identificação" },
            { TemaCheck.IluminacaoPublica, "Iluminação Pública" },
            { TemaCheck.SegurancaSinalizacao, "Segurança/Sinalização" },
            { TemaCheck.Compartilhamento, "Compartilhamento" },
            { TemaCheck.OutrasConstatacoes, "Outras Constatações" }
        };

    public static string ToDescricao(this TemaCheck tema)
        => _map.TryGetValue(tema, out var desc)
            ? desc
            : throw new ArgumentNullException("Tema Principal Não Mapeado");
}