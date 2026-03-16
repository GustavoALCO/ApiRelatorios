using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class TemaFiscalizacaoMapper
{
    private static readonly Dictionary<TemaFiscalizacao, string> _map =
        new()
        {
            { TemaFiscalizacao.Vegetacao, "Vegetação ao Alcance da Rede" },
            { TemaFiscalizacao.PostesDanificados, "Poste Danificado" },
            { TemaFiscalizacao.PostesDesalinhados, "Postes Desalinhados ou Fora de Prumo" },
            { TemaFiscalizacao.RedeComCondutoresDesivelados, "Rede com Condutores Desnivelados, Soltos ou com Espaçamentos Inadequados" },
            { TemaFiscalizacao.CruzetasDanificadas, "Cruzetas Danificadas ou Fora de Posição" },
            { TemaFiscalizacao.CruzetasForaDePosicao, "Cruzetas Fora de Posição da Bissetriz" },
            { TemaFiscalizacao.IsoladoresTipoPinoDesalinhados, "Isoladores Tipo Pino Desalinhados, Soltos ou Fletidos" },
            { TemaFiscalizacao.TransformadorComOxidacao, "Transformador com Oxidação, Corrosão ou Vazamento de Óleo" },
            { TemaFiscalizacao.InstalacaoDeEquipamentoSemUso, "Instalação de Equipamento sem Uso/Inativo" },
            { TemaFiscalizacao.ParaRaiosDanificados, "Para-raios Danificados, Ausentes ou Atuados" },
            { TemaFiscalizacao.EquipamentoSemIdentificacao, "Equipamento sem Identificação do Número Operativo" },
            { TemaFiscalizacao.Subestacao, "DIGITAR A SUBESTAÇÃO" },
            { TemaFiscalizacao.Diversos, "Diversos" }
        };

    public static string ToDescricao(this TemaFiscalizacao tema)
        => _map.TryGetValue(tema, out var desc)
            ? desc
            : throw new Exception("");
}