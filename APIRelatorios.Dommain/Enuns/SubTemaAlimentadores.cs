namespace APIRelatorios.Dommain.Enuns;

public enum SubTemaAlimentadores
{
    // Saída do Alimentador
    SalaBateriaRetificadores,
    PaineisDosAlimentadores,
    CubiculosDosDisjuntores,
    DiagramaUnifilar,
    AVCB,
    ChavesBloqueioManobra,
    PaneAlarmes,
    CabosCondutores,
    ChavesSeccionadoresParaRaios,
    EstruturasEIsoladores,
    ValoresGrandezasEletricasImportantes,

    // Vegetação
    VegetacaoEmContatoComRedeEletrica,
    VegetacaoAoAlcanceDaRede,
    RiscoQuedaVegetacaoSobreRede,

    // Postes
    PosteComFerragemExposta,
    PosteComAberturasNoConcreto,
    PosteConcretoQuebrado,
    PosteConcretoFletido,
    PostesDesalinhadosOuForaDePrumo,
    PosteSemEstabilidadeNaBase,
    LocacaoInadequadaDePoste,
    PosteMadeiraPodridaOcaOuComAberturas,

    // Cruzetas
    CruzetasDanificadas,
    CruzetasForaPosicaoBissetrizArriadasOuGiradas,
    IntegridadeSuporteCruzetas,

    // Isoladores
    IsoladoresComTrincasOuQuebras,
    IsoladoresComSujeiraOuFuligem,
    IsoladoresTortos,
    FixacaoIsoladoresNaCruzeta,

    // Condutores
    EstadoFisicoCondutores,
    TensaoMecanicaOuEspacamentoInadequado,
    AfastamentoArvoresEstruturas,
    InstalacaoSuportesEspacadores,
    AusenciaCaboNeutro,

    // Segurança
    CaboPartido,
    ProximidadeRedeComEdificacoes,
    CondutoresMetalicosProximosOuTocandoRede,

    // Aterramento
    PresencaEstadoCondutorAterramento,
    ConexoesCorretasEContinuas,

    // Transformadores
    VazamentosDeOleo,
    CorrosaoOxidacao,
    Estufamento,
    Fixacao,
    EstadoDasBuchas,
    NinhoDePassaro,
    RuidoAnormal,

    // Chaves/Religadores
    IntegridadeFisica,
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
    InstalacaoOuEquipamentoSemUsoInativo,

    // Equipamento sem Identificação
    EquipamentoSemNumeroOperativo,
    NumeroOperativoIlegivel,

    // Iluminação Pública
    EstadoDaLuminaria,
    FuncionamentoDaLampada,
    FotocelulaEmOperacao,
    FiacaoExpostaOuMalFixada,

    // Segurança/Sinalização
    PlacasAdvertenciaVisiveis,
    BarreirasProtecaoAdequadas,
    ConformidadeNormasNR10Concessionaria,

    // Compartilhamento
    PosicionamentoCorretoCabosAbaixoRede,
    AfastamentoMinimo,
    AusenciaCabosSoltosRompidosOuApoiados,
    FixacaoAdequadaCaixas,
    IdentificacaoResponsavelCabo,
    IndiciosOcupacaoClandestina,
    ExcessoTensionamentoCabos,
    OrganizacaoFeixeCabosPoste,

    // Outras constatações
    DescreverConstatacaoObservada
}