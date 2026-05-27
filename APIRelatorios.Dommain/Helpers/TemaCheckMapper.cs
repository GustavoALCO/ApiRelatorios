using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;
public static class TemaCheckMapper
{
    private static readonly Dictionary<TemaCheck, string> _map =
        new()
        {
            { TemaCheck.Aterramento, "Aterramento" },
            { TemaCheck.ChavesReligadores, "Chaves/Religadores" },
            { TemaCheck.Compartilhamento, "Compartilhamento" },
            { TemaCheck.Condutores, "Condutores" },
            { TemaCheck.EstruturasFerragens, "Estruturas/Ferragens" },
            { TemaCheck.IluminacaoPublica, "Iluminação Pública" },
            { TemaCheck.Isoladores, "Isoladores" },
            { TemaCheck.ParaRaios, "Para-Raios" },
            { TemaCheck.Postes, "Postes" },
            { TemaCheck.Seguranca, "Segurança/Sinalização" },
            { TemaCheck.Transformadores, "Transformadores" },
        };

    public static string ToDescricao(this TemaCheck tema)
        => _map.TryGetValue(tema, out var desc)
            ? desc
            : throw new Exception("");
}