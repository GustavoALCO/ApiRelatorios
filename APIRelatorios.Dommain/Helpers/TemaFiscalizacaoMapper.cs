using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;
public class TemaFiscalizacaoMapper
{
    private static readonly Dictionary<TemaCheck, (int Min, int Max)> _regras =
        new()
        {
            { TemaCheck.Postes, (0, 4) },
            { TemaCheck.Compartilhamento, (5, 11) },
            { TemaCheck.EstruturasFerragens, (12, 14) },
            { TemaCheck.Isoladores, (15, 16) },
            { TemaCheck.Condutores, (17, 19) },
            { TemaCheck.Aterramento, (20, 21) },
            { TemaCheck.Transformadores, (22, 25) },
            { TemaCheck.ChavesReligadores, (26, 28) },
            { TemaCheck.ParaRaios, (29, 31) },
            { TemaCheck.IluminacaoPublica, (32, 35) },
            { TemaCheck.Vegetacao, (36, 37) },
            { TemaCheck.Seguranca, (38, 40) },
            { TemaCheck.Outros, (41, 41) }
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

