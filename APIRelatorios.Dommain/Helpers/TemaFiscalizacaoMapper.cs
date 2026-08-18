using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class TemaFiscalizacaoMapper
{
    private static readonly Dictionary<TemaCheck, HashSet<SubTemaAlimentadores>> _regras =
        new()
        {
            {
                TemaCheck.SaidaDoAlimentador,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.SalaBateriaRetificadores,
                    SubTemaAlimentadores.PaineisDosAlimentadores,
                    SubTemaAlimentadores.CubiculosDosDisjuntores,
                    SubTemaAlimentadores.DiagramaUnifilar,
                    SubTemaAlimentadores.AVCB,
                    SubTemaAlimentadores.ChavesBloqueioManobra,
                    SubTemaAlimentadores.PaneAlarmes,
                    SubTemaAlimentadores.CabosCondutores,
                    SubTemaAlimentadores.ChavesSeccionadoresParaRaios,
                    SubTemaAlimentadores.EstruturasEIsoladores,
                    SubTemaAlimentadores.ValoresGrandezasEletricasImportantes
                }
            },

            {
                TemaCheck.Vegetacao,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.VegetacaoEmContatoComRedeEletrica,
                    SubTemaAlimentadores.VegetacaoAoAlcanceDaRede,
                    SubTemaAlimentadores.RiscoQuedaVegetacaoSobreRede
                }
            },

            {
                TemaCheck.Postes,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.PosteComFerragemExposta,
                    SubTemaAlimentadores.PosteComAberturasNoConcreto,
                    SubTemaAlimentadores.PosteConcretoQuebrado,
                    SubTemaAlimentadores.PosteConcretoFletido,
                    SubTemaAlimentadores.PostesDesalinhadosOuForaDePrumo,
                    SubTemaAlimentadores.PosteSemEstabilidadeNaBase,
                    SubTemaAlimentadores.LocacaoInadequadaDePoste,
                    SubTemaAlimentadores.PosteMadeiraPodridaOcaOuComAberturas,
                    SubTemaAlimentadores.PosteDeFibra
                }
            },

            {
                TemaCheck.Cruzetas,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.CruzetasDanificadas,
                    SubTemaAlimentadores.CruzetasForaPosicaoBissetrizArriadasOuGiradas,
                    SubTemaAlimentadores.IntegridadeSuporteCruzetas
                }
            },

            {
                TemaCheck.Isoladores,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.IsoladoresComTrincasOuQuebras,
                    SubTemaAlimentadores.IsoladoresComSujeiraOuFuligem,
                    SubTemaAlimentadores.IsoladoresTortos,
                    SubTemaAlimentadores.FixacaoIsoladoresNaCruzeta,
                    SubTemaAlimentadores.IsoladoresQuebrado,
                    SubTemaAlimentadores.AusenciaIsoladores,
                    SubTemaAlimentadores.IsoladorPolimero
                }
            },

            {
                TemaCheck.Condutores,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.EstadoFisicoCondutores,
                    SubTemaAlimentadores.TensaoMecanicaOuEspacamentoInadequado,
                    SubTemaAlimentadores.AfastamentoArvoresEstruturas,
                    SubTemaAlimentadores.InstalacaoSuportesEspacadores,
                    SubTemaAlimentadores.AusenciaCaboNeutro,
                    SubTemaAlimentadores.VaoMuitoGrande
                }
            },

            {
                TemaCheck.Seguranca,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.CaboPartido,
                    SubTemaAlimentadores.ProximidadeRedeComEdificacoes,
                    SubTemaAlimentadores.CondutoresMetalicosProximosOuTocandoRede,
                    SubTemaAlimentadores.RedeDentroDePropriedadeParticular
                }
            },

            {
                TemaCheck.Aterramento,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.PresencaEstadoCondutorAterramento,
                    SubTemaAlimentadores.ConexoesCorretasEContinuas
                }
            },

            {
                TemaCheck.Transformadores,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.VazamentosDeOleo,
                    SubTemaAlimentadores.CorrosaoOxidacao,
                    SubTemaAlimentadores.Estufamento,
                    SubTemaAlimentadores.Fixacao,
                    SubTemaAlimentadores.EstadoDasBuchas,
                    SubTemaAlimentadores.NinhoDePassaro,
                    SubTemaAlimentadores.RuidoAnormal
                }
            },

            {
                TemaCheck.ChavesReligadores,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.IntegridadeFisica,
                    SubTemaAlimentadores.ContatosEManobrabilidade,
                    SubTemaAlimentadores.SinalizacaoDePosicao,
                    SubTemaAlimentadores.Furto
                }
            },

            {
                TemaCheck.ParaRaios,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.AusenciaParaRaios,
                    SubTemaAlimentadores.ParaRaiosDanificados,
                    SubTemaAlimentadores.ParaRaiosAtuados,
                    SubTemaAlimentadores.FixacaoCorretaParaRaios,
                    SubTemaAlimentadores.SujeiraNoParaRaios,
                    SubTemaAlimentadores.ConexaoAoAterramento
                }
            },

            {
                TemaCheck.EquipamentoInativo,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.InstalacaoOuEquipamentoSemUsoInativo
                }
            },

            {
                TemaCheck.EquipamentoSemIdentificacao,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.EquipamentoSemNumeroOperativo,
                    SubTemaAlimentadores.NumeroOperativoIlegivel
                }
            },

            {
                TemaCheck.IluminacaoPublica,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.EstadoDaLuminaria,
                    SubTemaAlimentadores.FuncionamentoDaLampada,
                    SubTemaAlimentadores.FotocelulaEmOperacao,
                    SubTemaAlimentadores.FiacaoExpostaOuMalFixada
                }
            },

            {
                TemaCheck.SegurancaSinalizacao,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.PlacasAdvertenciaVisiveis,
                    SubTemaAlimentadores.BarreirasProtecaoAdequadas,
                    SubTemaAlimentadores.ConformidadeNormasNR10Concessionaria
                }
            },

            {
                TemaCheck.Compartilhamento,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.PosicionamentoCorretoCabosAbaixoRede,
                    SubTemaAlimentadores.AfastamentoMinimo,
                    SubTemaAlimentadores.AusenciaCabosSoltosRompidosOuApoiados,
                    SubTemaAlimentadores.FixacaoAdequadaCaixas,
                    SubTemaAlimentadores.IdentificacaoResponsavelCabo,
                    SubTemaAlimentadores.IndiciosOcupacaoClandestina,
                    SubTemaAlimentadores.ExcessoTensionamentoCabos,
                    SubTemaAlimentadores.OrganizacaoFeixeCabosPoste
                }
            },

            {
                TemaCheck.OutrasConstatacoes,
                new HashSet<SubTemaAlimentadores>
                {
                    SubTemaAlimentadores.DescreverConstatacaoObservada
                }
            }
        };

    public static bool ValidarSubTemas(
        TemaCheck tema,
        IEnumerable<SubTemaAlimentadores> subTemas)
    {
        if (!_regras.TryGetValue(tema, out var permitidos))
            return false;

        return subTemas.All(permitidos.Contains);
    }

    public static bool ValidarSubTema(
        TemaCheck tema,
        SubTemaAlimentadores subTema)
    {
        return _regras.TryGetValue(tema, out var permitidos)
            && permitidos.Contains(subTema);
    }

    public static IReadOnlyCollection<SubTemaAlimentadores> ObterSubTemas(
        TemaCheck tema)
    {
        if (!_regras.TryGetValue(tema, out var permitidos))
            return Array.Empty<SubTemaAlimentadores>();

        return permitidos;
    }

    public static string ObterMensagem(TemaCheck tema)
    {
        if (!_regras.TryGetValue(tema, out var permitidos))
            return "Tema inválido.";

        var nomes = permitidos
            .Select(x => x.ToString());

        return $"Os subtemas permitidos para {tema} são: {string.Join(", ", nomes)}.";
    }
}