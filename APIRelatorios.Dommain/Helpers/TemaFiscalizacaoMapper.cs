using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;
public class TemaFiscalizacaoMapper
{
    private static readonly Dictionary<TemaCheck, (int Min, int Max)> _regras =
    new()
    {
    { TemaCheck.SaidaDoAlimentador, (0, 10) },

    { TemaCheck.Vegetacao, (11, 13) },

    { TemaCheck.Postes, (14, 21) },

    { TemaCheck.Cruzetas, (22, 24) },

    { TemaCheck.Isoladores, (25, 28) },

    { TemaCheck.Condutores, (29, 33) },

    { TemaCheck.Seguranca, (34, 36) },

    { TemaCheck.Aterramento, (37, 38) },

    { TemaCheck.Transformadores, (39, 45) },

    { TemaCheck.ChavesReligadores, (46, 48) },

    { TemaCheck.ParaRaios, (49, 54) },

    { TemaCheck.EquipamentoInativo, (55, 55) },

    { TemaCheck.EquipamentoSemIdentificacao, (56, 57) },

    { TemaCheck.IluminacaoPublica, (58, 61) },

    { TemaCheck.SegurancaSinalizacao, (62, 64) },

    { TemaCheck.Compartilhamento, (65, 72) },

    { TemaCheck.OutrasConstatacoes, (73, 73) }
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
            $"Os temas permitidos para {tema} são de {faixa.Min} até {faixa.Max}.";
    }
}

