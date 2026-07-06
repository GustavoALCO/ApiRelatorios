using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class SubTemaAlimentadoresMapper
{
    private static readonly Dictionary<SubTemaAlimentadores, string> _map =
        new()
        {
            // Saída do Alimentador
            { SubTemaAlimentadores.SalaBateriaRetificadores, "Sala de bateria/retificadores com irregularidades" },
            { SubTemaAlimentadores.PaineisDosAlimentadores, "Painéis dos alimentadores com irregularidades" },
            { SubTemaAlimentadores.CubiculosDosDisjuntores, "Cubículos dos disjuntores com irregularidades" },
            { SubTemaAlimentadores.DiagramaUnifilar, "Diagrama unifilar ausente ou desatualizado" },
            { SubTemaAlimentadores.AVCB, "AVCB ausente ou vencido" },
            { SubTemaAlimentadores.ChavesBloqueioManobra, "Falhas nas chaves de bloqueio/manobra" },
            { SubTemaAlimentadores.PaneAlarmes, "Existência de panes/alarmes injustificadas" },
            { SubTemaAlimentadores.CabosCondutores, "Cabos condutores danificados" },
            { SubTemaAlimentadores.ChavesSeccionadoresParaRaios, "Chaves seccionadoras ou para-raios com irregularidades" },
            { SubTemaAlimentadores.EstruturasEIsoladores, "Estruturas ou isoladores com irregularidades" },
            { SubTemaAlimentadores.ValoresGrandezasEletricasImportantes, "Valores de grandezas elétricas fora do esperado" },

            // Vegetação
            { SubTemaAlimentadores.VegetacaoEmContatoComRedeEletrica, "Vegetação em contato com a rede elétrica" },
            { SubTemaAlimentadores.VegetacaoAoAlcanceDaRede, "Vegetação ao alcance da rede elétrica" },
            { SubTemaAlimentadores.RiscoQuedaVegetacaoSobreRede, "Risco de queda de vegetação sobre a rede" },

            // Postes
            { SubTemaAlimentadores.PosteComFerragemExposta, "Poste com ferragem exposta" },
            { SubTemaAlimentadores.PosteComAberturasNoConcreto, "Poste com aberturas (fissura, trinca ou rachadura) no concreto" },
            { SubTemaAlimentadores.PosteConcretoQuebrado, "Poste de concreto quebrado" },
            { SubTemaAlimentadores.PosteConcretoFletido, "Poste de concreto fletido" },
            { SubTemaAlimentadores.PostesDesalinhadosOuForaDePrumo, "Poste desalinhado ou fora de prumo" },
            { SubTemaAlimentadores.PosteSemEstabilidadeNaBase, "Poste com estabilidade na base comprometida" },
            { SubTemaAlimentadores.LocacaoInadequadaDePoste, "Poste instalado em local inadequado" },
            { SubTemaAlimentadores.PosteMadeiraPodridaOcaOuComAberturas, "Poste de madeira com podridão, oco ou com aberturas" },

            // Cruzetas
            { SubTemaAlimentadores.CruzetasDanificadas, "Cruzeta danificada" },
            { SubTemaAlimentadores.CruzetasForaPosicaoBissetrizArriadasOuGiradas, "Cruzeta fora da posição da bissetriz, arriada ou girada" },
            { SubTemaAlimentadores.IntegridadeSuporteCruzetas, "Suporte da cruzeta comprometido" },

            // Isoladores
            { SubTemaAlimentadores.IsoladoresComTrincasOuQuebras, "Isoladores com trincas ou quebrado" },
            { SubTemaAlimentadores.IsoladoresComSujeiraOuFuligem, "Isoladores com sujeira ou fuligem" },
            { SubTemaAlimentadores.IsoladoresTortos, "Isoladores desalinhados ou fletidos" },
            { SubTemaAlimentadores.FixacaoIsoladoresNaCruzeta, "Isolador com fixação na cruzeta comprometida" },

            // Condutores
            { SubTemaAlimentadores.EstadoFisicoCondutores, "Condutor com danos físicos" },
            { SubTemaAlimentadores.TensaoMecanicaOuEspacamentoInadequado, "Tensão mecânica ou espaçamento inadequado entre os condutores" },
            { SubTemaAlimentadores.AfastamentoArvoresEstruturas, "Afastamento inadequado de árvores ou estruturas" },
            { SubTemaAlimentadores.InstalacaoSuportesEspacadores, "Suportes ou espaçadores instalados inadequadamente, soltos, quebrados ou ausentes" },
            { SubTemaAlimentadores.AusenciaCaboNeutro, "Ausência de cabo neutro" },

            // Segurança
            { SubTemaAlimentadores.CaboPartido, "Cabo partido" },
            { SubTemaAlimentadores.ProximidadeRedeComEdificacoes, "Rede elétrica próxima de edificações ou de estruturas" },
            { SubTemaAlimentadores.CondutoresMetalicosProximosOuTocandoRede, "Condutores metálicos próximos ou tocando a rede elétrica" },

            // Aterramento
            { SubTemaAlimentadores.PresencaEstadoCondutorAterramento, "Condutor de aterramento ausente ou em más condições" },
            { SubTemaAlimentadores.ConexoesCorretasEContinuas, "Conexões de aterramento inadequadas ou interrompidas" },

            // Transformadores
            { SubTemaAlimentadores.VazamentosDeOleo, "Transformador com vazamento de óleo" },
            { SubTemaAlimentadores.CorrosaoOxidacao, "Transformador com corrosão ou oxidação" },
            { SubTemaAlimentadores.Estufamento, "Transformador com estufamento" },
            { SubTemaAlimentadores.Fixacao, "Transformador com fixação inadequada" },
            { SubTemaAlimentadores.EstadoDasBuchas, "Buchas do transformador danificadas" },
            { SubTemaAlimentadores.NinhoDePassaro, "Transformador com presença de ninho de pássaro em pontos energizados" },
            { SubTemaAlimentadores.RuidoAnormal, "Transformador com ruído anormal" },

            // Chaves / Religadores
            { SubTemaAlimentadores.IntegridadeFisica, "Chave ou religador com integridade física comprometida" },
            { SubTemaAlimentadores.ContatosEManobrabilidade, "Chave ou religador com problemas nos contatos ou na manobrabilidade" },
            { SubTemaAlimentadores.SinalizacaoDePosicao, "Chave ou religador com sinalização de posição inadequada" },

            // Para-raios
            { SubTemaAlimentadores.AusenciaParaRaios, "Ausência de para-raios" },
            { SubTemaAlimentadores.ParaRaiosDanificados, "Para-raios danificados" },
            { SubTemaAlimentadores.ParaRaiosAtuados, "Para-raios atuados" },
            { SubTemaAlimentadores.FixacaoCorretaParaRaios, "Para-raios com fixação inadequada" },
            { SubTemaAlimentadores.SujeiraNoParaRaios, "Para-raios com excesso de sujeira" },
            { SubTemaAlimentadores.ConexaoAoAterramento, "Conexão do para-raios ao aterramento inadequada ou inexistente" },

            // Equipamento Inativo
            { SubTemaAlimentadores.InstalacaoOuEquipamentoSemUsoInativo, "Instalação ou equipamento sem uso (inativo)" },

            // Equipamento sem Identificação
            { SubTemaAlimentadores.EquipamentoSemNumeroOperativo, "Número operativo ilegível ou danificado" },
            { SubTemaAlimentadores.NumeroOperativoIlegivel, "Número operativo ilegível" },

            // Iluminação Pública
            { SubTemaAlimentadores.EstadoDaLuminaria, "Luminária em mau estado de conservação" },
            { SubTemaAlimentadores.FuncionamentoDaLampada, "Lâmpada queimada ou acesa durante o dia" },
            { SubTemaAlimentadores.FotocelulaEmOperacao, "Fotocélula com defeito" },
            { SubTemaAlimentadores.FiacaoExpostaOuMalFixada, "Fiação exposta ou mal fixada" },

            // Segurança / Sinalização
            { SubTemaAlimentadores.PlacasAdvertenciaVisiveis, "Ausência ou má conservação das placas de advertência" },
            { SubTemaAlimentadores.BarreirasProtecaoAdequadas, "Barreiras de proteção inadequadas" },
            { SubTemaAlimentadores.ConformidadeNormasNR10Concessionaria, "Não conformidade com a NR-10 ou normas da concessionária" },

            // Compartilhamento
            { SubTemaAlimentadores.PosicionamentoCorretoCabosAbaixoRede, "Cabos de compartilhamento posicionados inadequadamente" },
            { SubTemaAlimentadores.AfastamentoMinimo, "Afastamento mínimo em redes não atendido" },
            { SubTemaAlimentadores.AusenciaCabosSoltosRompidosOuApoiados, "Cabos soltos, rompidos ou apoiados inadequadamente" },
            { SubTemaAlimentadores.FixacaoAdequadaCaixas, "Caixas com fixação inadequada" },
            { SubTemaAlimentadores.IdentificacaoResponsavelCabo, "Ausência de identificação do responsável pelo cabo" },
            { SubTemaAlimentadores.IndiciosOcupacaoClandestina, "Indícios de ocupação clandestina" },
            { SubTemaAlimentadores.ExcessoTensionamentoCabos, "Cabos com excesso de tensionamento" },
            { SubTemaAlimentadores.OrganizacaoFeixeCabosPoste, "Feixe de cabos desorganizado" },

            // Outras Constatações
            { SubTemaAlimentadores.DescreverConstatacaoObservada, "Outra não conformidade identificada" }
        };


    public static string ToDescricao(this SubTemaAlimentadores tema)
    => _map.TryGetValue(tema, out var desc)
        ? desc
        : throw new ArgumentNullException("Erro ao Mapear os SubTemas");
}