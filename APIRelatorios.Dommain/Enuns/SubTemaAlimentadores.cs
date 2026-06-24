namespace APIRelatorios.Dommain.Enuns;

public enum SubTemaAlimentadores
{
    // Saída do Alimentador
    SalaBateriaRetificadores,
    PaineisDosAlimentadores,
    CubiculosDosDisjuntores,
    DiagramaUnifilar,
    AVCB,
    ChavesBloqueioManobraPaneAlarmes,
    CabosCondutores,
    ChavesSeccionadorasParaRaios,
    EstruturasEIsoladores,

    // Vegetação
    VegetacaoEmContatoComRedeEletrica,
    VegetacaoAoAlcanceDaRede,
    RiscoQuedaVegetacaoSobreRede,

    // Postes
    EstadoFisicoPostes,
    PosteComAberturasConcreto,
    PosteConcretoQuebrado,
    PosteConcretoFletido,
    PostesDesalinhadosOuForaDePrumo,
    PosteSemEstabilidadeNaBase,
    PosteMadeiraComPodridaoOuOco,

    // Cruzetas
    CruzetasDanificadas,
    CruzetasForaPosicaoBissetriz,
    IntegridadeSuporteCruzetas,

    // Isoladores
    IsoladoresTrincadosOuQuebrados,
    IsoladoresComSujeiraOuFuligem,
    FixacaoIsoladoresNaCruzeta,

    // Condutores
    EstadoFisicoCondutores,
    TensaoMecanicaOuEspacamentoInadequado,
    AfastamentoArvoresEstruturas,
    InstalacaoSuportesSeparadores,
    AusenciaCaboNeutro,

    // Segurança
    CaboPartido,
    ProximidadeRedeComEdificacoes,
    CondutoresTelecomProximosDaRede,

    // Aterramento
    PresencaCondutorAterramento,
    ConexoesCorretasEContinuas,

    // Transformadores
    VazamentosDeOleo,
    CorrosaoOuOxidacao,
    EstufamentoTransformador,
    FixacaoTransformador,
    EstadoDasBuchas,
    NinhoDePassaro,
    RuidoAnormal,

    // Chaves / Religadores
    IntegridadeFisicaChavesReligadores,
    ContatosEManobrabilidade,
    SinalizacaoDePosicao,

    // Para-raios
    AusenciaParaRaios,
    ParaRaiosDanificados,
    ParaRaiosAtuados,
    FixacaoCorretaParaRaios,
    SujeiraNoParaRaios,
    ConexaoAoAterramento,

    // Equipamento Inativo
    EquipamentoSemUsoOuInativo,

    // Equipamento sem Identificação
    EquipamentoSemNumeroOperativo,
    NumeroOperativoIlegivel,

    // Iluminação Pública
    EstadoDaLuminaria,
    FuncionamentoDaLampada,
    FotocelulaEmOperacao,
    FiacaoExpostaOuMalFixada,

    // Segurança / Sinalização
    PlacasAdvertenciaVisiveis,
    BarreirasProtecaoAdequadas,
    ConformidadeNormasNR10,

    // Compartilhamento
    PosicionamentoCorretoCabos,
    AfastamentoMinimo,
    AusenciaCabosSoltosOuRompidos,
    FixacaoAdequadaCaixas,
    IdentificacaoResponsavelCabo,
    IndiciosOcupacaoClandestina,
    ExcessoTensionamentoCabos,
    OrganizacaoFeixeCabos,

    // Outras Constatações
    OutraConstatacao
}
