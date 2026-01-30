using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class TemaFiscalizacaoMapper
{
    private static readonly Dictionary<TemaFiscalizacao, string> _map =
        new()
        {
            { TemaFiscalizacao.Vegetacao, "Vegetação ao Alcanse das Redes" },
            { TemaFiscalizacao.PostesDanificados, "Poste Danificado" },
            { TemaFiscalizacao.PostesDesalinhados, "Postes Desalinhados ou Fora de Prumo" },
            { TemaFiscalizacao.RedeComCondutoresDesivelados, "Rede com Condutores Desnivelados, Soltos ou com espaçamentos inadequados" },
            { TemaFiscalizacao.CruzetasDanificadas, "Cruzetas Danificadas ou Fora de Posição" },
            { TemaFiscalizacao.CruzetasForaDePosicao, "Cruzetas Fora de Posição da Bissetriz" },
            { TemaFiscalizacao.IsoladoresTipoPinoDesalinhados, "Isoladores Tipo Pino Desalinhados, Soltos ou Fletidos" },
            { TemaFiscalizacao.TransformadorComOxidacao, "Transformador com Oxidação, Corrosão ou vazamento de Óleo" },
            { TemaFiscalizacao.InstalacaoDeEquipamentoSemUso, "Instalação de Equipamento Sem Uso/Inativos" },
            { TemaFiscalizacao.ParaRaiosDanificados, "Para-Raios Danificados, Ausente ou Atuados" },
            { TemaFiscalizacao.EquipamentoSemIdentificacao, "Equipamento sem Identificação do Numero Operativo" },
            { TemaFiscalizacao.Subestacao, "DIGITAR A SUBSTAÇÃO" },
            { TemaFiscalizacao.Diversos, "Diversos" }
        };

    public static string ToDescricao(this TemaFiscalizacao tema)
        => _map.TryGetValue(tema, out var desc)
            ? desc
            : throw new Exception("");
}
