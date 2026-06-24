using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;
public class TemaFiscalizacaoMapper
{
    private static readonly Dictionary<TemaCheck, (int Min, int Max)> _regras =
    new()
    {
        { TemaCheck.SaidaDoAlimentador, (0, 8) },

        { TemaCheck.Vegetacao, (9, 11) },

        { TemaCheck.Postes, (12, 18) },

        { TemaCheck.Cruzetas, (19, 21) },

        { TemaCheck.Isoladores, (22, 24) },

        { TemaCheck.Condutores, (25, 29) },

        { TemaCheck.Seguranca, (30, 32) },

        { TemaCheck.Aterramento, (33, 34) },

        { TemaCheck.Transformadores, (35, 41) },

        { TemaCheck.ChavesReligadores, (42, 44) },

        { TemaCheck.ParaRaios, (45, 50) },

        { TemaCheck.EquipamentoInativo, (51, 51) },

        { TemaCheck.EquipamentoSemIdentificacao, (52, 53) },

        { TemaCheck.IluminacaoPublica, (54, 57) },

        { TemaCheck.SegurancaSinalizacao, (58, 60) },

        { TemaCheck.Compartilhamento, (61, 68) },

        { TemaCheck.OutrasConstatacoes, (69, 69) }
    };

    public static bool ValidarSubTemas(
        TemaCheck tema,
        IEnumerable<SubTemaAlimentadores> subTemas)
    {
        if (!_regras.TryGetValue(tema, out var faixa))
            return false;

        return subTemas.All(subtema =>
            (int)subtema >= faixa.Min &&
            (int)subtema <= faixa.Max);
    }

    public static string ObterMensagem(
        TemaCheck tema)
    {
        if (!_regras.TryGetValue(tema, out var faixa))
            return "Tema inválido.";

        return
            $"Os subtemas permitidos para {tema} são de {faixa.Min} até {faixa.Max}.";
    }
}

