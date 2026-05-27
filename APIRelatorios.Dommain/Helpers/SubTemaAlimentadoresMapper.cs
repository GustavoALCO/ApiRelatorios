using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class SubTemaAlimentadoresMapper
{
    private static readonly Dictionary<SubTemaAlimentadores, string> _map =
        new()
        {
            { SubTemaAlimentadores.EstadoFisico, "Não conformidade no estado físico" },

            { SubTemaAlimentadores.EstabilidadeEAlinhamento, "Não conformidade na estabilidade ou alinhamento" },

            { SubTemaAlimentadores.IdentificacaoLegivel, "Não conformidade na identificação" },

            { SubTemaAlimentadores.SinaisdeColisao_Impacto, "Sinais de colisão ou impacto" },

            { SubTemaAlimentadores.CondicoesDaBase, "Não conformidade nas condições da base" },

            { SubTemaAlimentadores.PosicionamentoCorretoDosCabos, "Não conformidade no posicionamento dos cabos" },

            { SubTemaAlimentadores.AfastamentoMinimo, "Não conformidade no afastamento mínimo" },

            { SubTemaAlimentadores.AusenciaDeCabosSoltos, "Cabos soltos, rompidos ou apoiados em ferragens" },

            { SubTemaAlimentadores.FixacaoAdequada, "Não conformidade na fixação" },

            { SubTemaAlimentadores.IdentificacaoResponsavel, "Ausência de identificação do responsável" },

            { SubTemaAlimentadores.OcupacaoClandestina, "Indícios de ocupação clandestina" },

            { SubTemaAlimentadores.OrganizacaoDoFeixeDeCabosNoPoste, "Não conformidade na organização do feixe de cabos" },

            { SubTemaAlimentadores.IntegridadeCruzetasSuportes, "Não conformidade na integridade estrutural" },

            { SubTemaAlimentadores.ApertoDePorcasEParafusos, "Não conformidade no aperto de porcas e parafusos" },

            { SubTemaAlimentadores.CorrosaoDesgaste, "Corrosão ou desgaste" },

            { SubTemaAlimentadores.TrincasQuebras, "Trincas, quebras ou fuligem" },

            { SubTemaAlimentadores.FixacaoCruzeta, "Não conformidade na fixação da cruzeta" },

            { SubTemaAlimentadores.EstadoFisicoDesgaste, "Desgaste ou emendas irregulares" },

            { SubTemaAlimentadores.AfastamentoEstruturas, "Não conformidade no afastamento de estruturas" },

            { SubTemaAlimentadores.PresencaEstado, "Não conformidade no aterramento" },

            { SubTemaAlimentadores.ConexoesCorretas, "Não conformidade nas conexões" },

            { SubTemaAlimentadores.VazamentoCorrosao, "Vazamentos, corrosão ou falhas na pintura" },

            { SubTemaAlimentadores.NivelOleo, "Não conformidade no nível de óleo" },

            { SubTemaAlimentadores.EstadoBuchas, "Não conformidade nas buchas" },

            { SubTemaAlimentadores.ParaRaios, "Não conformidade nos para-raios" },

            { SubTemaAlimentadores.IntegridadeFisica, "Não conformidade na integridade física" },

            { SubTemaAlimentadores.ContatosManobrabilidade, "Não conformidade nos contatos ou manobrabilidade" },

            { SubTemaAlimentadores.SinalizacaoPosicao, "Não conformidade na sinalização" },

            { SubTemaAlimentadores.FixacaoCorreta, "Não conformidade na fixação" },

            { SubTemaAlimentadores.TrincasQueimaduras, "Trincas, queimaduras ou fuligem" },

            { SubTemaAlimentadores.ConexaoAterramento, "Não conformidade na conexão de aterramento" },

            { SubTemaAlimentadores.EstadoLuminaria, "Não conformidade na luminária" },

            { SubTemaAlimentadores.FuncionamentoLampada, "Não conformidade no funcionamento da lâmpada" },

            { SubTemaAlimentadores.FotocelulaOperacao, "Não conformidade na fotocélula" },

            { SubTemaAlimentadores.FiacaoExposta, "Fiação exposta ou mal fixada" },

            { SubTemaAlimentadores.GalhosProximos, "Galhos próximos à rede elétrica" },

            { SubTemaAlimentadores.RiscoDeQueda, "Risco de queda" },

            { SubTemaAlimentadores.PlacasAdvertenciaVisiveis, "Não conformidade na sinalização de advertência" },

            { SubTemaAlimentadores.BarreirasAdequadas, "Não conformidade nas barreiras de proteção" },

            { SubTemaAlimentadores.ConformidadeNormas, "Não conformidade com normas técnicas e regulamentadoras" },
            
            { SubTemaAlimentadores.TensaoMecanicaAdequada, "Não conformidade na tensão mecânica" },

            { SubTemaAlimentadores.Outros, "Outras não conformidades identificadas" },
        };

    public static string ToDescricao(this SubTemaAlimentadores tema)
    {
        if (_map.TryGetValue(tema, out var desc))
        {
            return desc;
        }

        return $"SubTema não mapeado ({(int)tema})";
    }
}