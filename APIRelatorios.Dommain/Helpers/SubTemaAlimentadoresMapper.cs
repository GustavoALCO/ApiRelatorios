using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class SubTemaAlimentadoresMapper
{
    private static readonly Dictionary<SubTemaAlimentadores, string> _map =
        new()
        {
            // Saída do Alimentador
            { SubTemaAlimentadores.SalaBateriaRetificadores, "Sala de bateria/retificadores" },
            { SubTemaAlimentadores.PaineisDosAlimentadores, "Painéis dos alimentadores" },
            { SubTemaAlimentadores.CubiculosDosDisjuntores, "Cubículos dos disjuntores" },
            { SubTemaAlimentadores.DiagramaUnifilar, "Diagrama unifilar" },
            { SubTemaAlimentadores.AVCB, "AVCB" },
            { SubTemaAlimentadores.ChavesBloqueioManobraPaneAlarmes, "Chaves de bloqueio/manobra, pane/alarmes" },
            { SubTemaAlimentadores.CabosCondutores, "Cabos condutores" },
            { SubTemaAlimentadores.ChavesSeccionadorasParaRaios, "Chaves seccionadoras/para-raios" },
            { SubTemaAlimentadores.EstruturasEIsoladores, "Estruturas e isoladores" },

            // Vegetação
            { SubTemaAlimentadores.VegetacaoEmContatoComRedeEletrica, "Vegetação em contato com a rede elétrica" },
            { SubTemaAlimentadores.VegetacaoAoAlcanceDaRede, "Vegetação ao alcance da rede" },
            { SubTemaAlimentadores.RiscoQuedaVegetacaoSobreRede, "Risco de queda de vegetação sobre a rede" },

            // Postes
            { SubTemaAlimentadores.EstadoFisicoPostes, "Estado físico" },
            { SubTemaAlimentadores.PosteComAberturasConcreto, "Poste com aberturas no concreto" },
            { SubTemaAlimentadores.PosteConcretoQuebrado, "Poste de concreto quebrado" },
            { SubTemaAlimentadores.PosteConcretoFletido, "Poste de concreto fletido" },
            { SubTemaAlimentadores.PostesDesalinhadosOuForaDePrumo, "Postes desalinhados ou fora de prumo" },
            { SubTemaAlimentadores.PosteSemEstabilidadeNaBase, "Poste sem estabilidade na base" },
            { SubTemaAlimentadores.PosteMadeiraComPodridaoOuOco, "Poste de madeira com podridão ou oco" },

            // Cruzetas
            { SubTemaAlimentadores.CruzetasDanificadas, "Cruzetas danificadas" },
            { SubTemaAlimentadores.CruzetasForaPosicaoBissetriz, "Cruzetas fora da posição da bissetriz" },
            { SubTemaAlimentadores.IntegridadeSuporteCruzetas, "Integridade de suporte das cruzetas" },

            // Isoladores
            { SubTemaAlimentadores.IsoladoresTrincadosOuQuebrados, "Trincas ou quebras" },
            { SubTemaAlimentadores.IsoladoresComSujeiraOuFuligem, "Sujeira ou fuligem" },
            { SubTemaAlimentadores.FixacaoIsoladoresNaCruzeta, "Fixação na cruzeta" },

            // Condutores
            { SubTemaAlimentadores.EstadoFisicoCondutores, "Estado físico dos condutores" },
            { SubTemaAlimentadores.TensaoMecanicaOuEspacamentoInadequado, "Tensão mecânica ou espaçamento inadequado" },
            { SubTemaAlimentadores.AfastamentoArvoresEstruturas, "Afastamento de árvores e estruturas" },
            { SubTemaAlimentadores.InstalacaoSuportesSeparadores, "Instalação correta de suportes e separadores" },
            { SubTemaAlimentadores.AusenciaCaboNeutro, "Ausência de cabo neutro" },

            // Segurança
            { SubTemaAlimentadores.CaboPartido, "Cabo partido" },
            { SubTemaAlimentadores.ProximidadeRedeComEdificacoes, "Proximidade da rede elétrica com edificações" },
            { SubTemaAlimentadores.CondutoresTelecomProximosDaRede, "Condutores de telecom próximos da rede elétrica" },

            // Aterramento
            { SubTemaAlimentadores.PresencaCondutorAterramento, "Presença e estado do condutor de aterramento" },
            { SubTemaAlimentadores.ConexoesCorretasEContinuas, "Conexões corretas e contínuas" },

            // Transformadores
            { SubTemaAlimentadores.VazamentosDeOleo, "Vazamentos de óleo" },
            { SubTemaAlimentadores.CorrosaoOuOxidacao, "Corrosão ou oxidação" },
            { SubTemaAlimentadores.EstufamentoTransformador, "Estufamento" },
            { SubTemaAlimentadores.FixacaoTransformador, "Fixação" },
            { SubTemaAlimentadores.EstadoDasBuchas, "Estado das buchas" },
            { SubTemaAlimentadores.NinhoDePassaro, "Ninho de pássaro" },
            { SubTemaAlimentadores.RuidoAnormal, "Ruído anormal" },

            // Chaves / Religadores
            { SubTemaAlimentadores.IntegridadeFisicaChavesReligadores, "Integridade física" },
            { SubTemaAlimentadores.ContatosEManobrabilidade, "Contatos e manobrabilidade" },
            { SubTemaAlimentadores.SinalizacaoDePosicao, "Sinalização de posição" },

            // Para-raios
            { SubTemaAlimentadores.AusenciaParaRaios, "Ausência de para-raios" },
            { SubTemaAlimentadores.ParaRaiosDanificados, "Para-raios danificados" },
            { SubTemaAlimentadores.ParaRaiosAtuados, "Para-raios atuados" },
            { SubTemaAlimentadores.FixacaoCorretaParaRaios, "Fixação correta" },
            { SubTemaAlimentadores.SujeiraNoParaRaios, "Sujeira no para-raios" },
            { SubTemaAlimentadores.ConexaoAoAterramento, "Conexão ao aterramento" },

            // Equipamentos
            { SubTemaAlimentadores.EquipamentoSemUsoOuInativo, "Equipamento sem uso ou inativo" },
            { SubTemaAlimentadores.EquipamentoSemNumeroOperativo, "Equipamento sem número operativo" },
            { SubTemaAlimentadores.NumeroOperativoIlegivel, "Número operativo ilegível" },

            // Iluminação Pública
            { SubTemaAlimentadores.EstadoDaLuminaria, "Estado da luminária" },
            { SubTemaAlimentadores.FuncionamentoDaLampada, "Funcionamento da lâmpada" },
            { SubTemaAlimentadores.FotocelulaEmOperacao, "Fotocélula em operação" },
            { SubTemaAlimentadores.FiacaoExpostaOuMalFixada, "Fiação exposta ou mal fixada" },

            // Segurança/Sinalização
            { SubTemaAlimentadores.PlacasAdvertenciaVisiveis, "Placas de advertência visíveis" },
            { SubTemaAlimentadores.BarreirasProtecaoAdequadas, "Barreiras de proteção adequadas" },
            { SubTemaAlimentadores.ConformidadeNormasNR10, "Conformidade com normas NR-10" },

            // Compartilhamento
            { SubTemaAlimentadores.PosicionamentoCorretoCabos, "Posicionamento correto dos cabos" },
            { SubTemaAlimentadores.AfastamentoMinimo, "Afastamento mínimo" },
            { SubTemaAlimentadores.AusenciaCabosSoltosOuRompidos, "Ausência de cabos soltos ou rompidos" },
            { SubTemaAlimentadores.FixacaoAdequadaCaixas, "Fixação adequada de caixas" },
            { SubTemaAlimentadores.IdentificacaoResponsavelCabo, "Identificação do responsável pelo cabo" },
            { SubTemaAlimentadores.IndiciosOcupacaoClandestina, "Indícios de ocupação clandestina" },
            { SubTemaAlimentadores.ExcessoTensionamentoCabos, "Excesso de tensionamento dos cabos" },
            { SubTemaAlimentadores.OrganizacaoFeixeCabos, "Organização do feixe de cabos" },

            // Outros
            { SubTemaAlimentadores.OutraConstatacao, "Outras constatações" }
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