namespace APIRelatorios.Dommain.Enuns;

public enum SubTemaAlimentadores
{
    // ===========================================================================================================
    // IMPORTANTE: NÃO REMOVA OS VALORES DESTE ENUM.
    //
    // A remoção ou alteração dos valores existentes pode comprometer o mapeamento dos temas e a compatibilidade
    // com os registros já armazenados.
    //
    // Para adicionar um novo item:
    //
    // 1. Utilize um valor numérico novo e exclusivo.
    // 2. Adicione o mapeamento do tema principal em Helpers/TemaFiscalizacaoMapper.cs.
    // 3. Adicione a descrição correspondente em Helpers/SubTemaAlimentadoresMapper.cs.
    //
    // Mantenha os valores existentes inalterados para preservar a integridade dos mapeamentos.
    // ===========================================================================================================

    // Saída do Alimentador
    SalaBateriaRetificadores = 0,
    PaineisDosAlimentadores = 1,
    CubiculosDosDisjuntores = 2,
    DiagramaUnifilar = 3,
    AVCB = 4,
    ChavesBloqueioManobra = 5,
    PaneAlarmes = 6,
    CabosCondutores = 7,
    ChavesSeccionadoresParaRaios = 8,
    EstruturasEIsoladores = 9,
    ValoresGrandezasEletricasImportantes = 10,

    // Vegetação
    VegetacaoEmContatoComRedeEletrica = 11,
    VegetacaoAoAlcanceDaRede = 12,
    RiscoQuedaVegetacaoSobreRede = 13,

    // Postes
    PosteComFerragemExposta = 14,
    PosteComAberturasNoConcreto = 15,
    PosteConcretoQuebrado = 16,
    PosteConcretoFletido = 17,
    PostesDesalinhadosOuForaDePrumo = 18,
    PosteSemEstabilidadeNaBase = 19,
    LocacaoInadequadaDePoste = 20,
    PosteMadeiraPodridaOcaOuComAberturas = 21,
    PosteDeFibra = 22,

    // Cruzetas
    CruzetasDanificadas = 23,
    CruzetasForaPosicaoBissetrizArriadasOuGiradas = 24,
    IntegridadeSuporteCruzetas = 25,

    // Isoladores
    IsoladoresComTrincasOuQuebras = 26,
    IsoladoresComSujeiraOuFuligem = 27,
    IsoladoresTortos = 28,
    FixacaoIsoladoresNaCruzeta = 29,
    IsoladoresQuebrado = 30,
    AusenciaIsoladores = 31,
    IsoladorPolimero = 32,


    // Condutores
    EstadoFisicoCondutores = 33,
    TensaoMecanicaOuEspacamentoInadequado = 34,
    AfastamentoArvoresEstruturas = 35,
    InstalacaoSuportesEspacadores = 36,
    AusenciaCaboNeutro = 37,
    VaoMuitoGrande = 38,

    // Segurança
    CaboPartido = 39,
    ProximidadeRedeComEdificacoes = 40,
    CondutoresMetalicosProximosOuTocandoRede = 41,
    RedeDentroDePropriedadeParticular = 42,

    // Aterramento
    PresencaEstadoCondutorAterramento = 43,
    ConexoesCorretasEContinuas = 44,

    // Transformadores
    VazamentosDeOleo = 45,
    CorrosaoOxidacao = 46,
    Estufamento = 47,
    Fixacao = 48,
    EstadoDasBuchas = 49,
    NinhoDePassaro = 50,
    RuidoAnormal = 51,

    // Chaves/Religadores
    IntegridadeFisica = 52,
    ContatosEManobrabilidade = 53,
    SinalizacaoDePosicao = 54,
    Furto = 55,

    // Para-raios
    AusenciaParaRaios = 56,
    ParaRaiosDanificados = 57,
    ParaRaiosAtuados = 58,
    FixacaoCorretaParaRaios = 59,
    SujeiraNoParaRaios = 60,
    ConexaoAoAterramento = 61,

    // Equipamento Inativo
    InstalacaoOuEquipamentoSemUsoInativo = 62,

    // Equipamento sem Identificação
    EquipamentoSemNumeroOperativo = 63,
    NumeroOperativoIlegivel = 64,

    // Iluminação Pública
    EstadoDaLuminaria = 65,
    FuncionamentoDaLampada = 66,
    FotocelulaEmOperacao = 67,
    FiacaoExpostaOuMalFixada = 68,

    // Segurança/Sinalização
    PlacasAdvertenciaVisiveis = 69,
    BarreirasProtecaoAdequadas = 70,
    ConformidadeNormasNR10Concessionaria = 71,

    // Compartilhamento
    PosicionamentoCorretoCabosAbaixoRede = 72,
    AfastamentoMinimo = 73,
    AusenciaCabosSoltosRompidosOuApoiados = 74,
    FixacaoAdequadaCaixas = 75,
    IdentificacaoResponsavelCabo = 76,
    IndiciosOcupacaoClandestina = 77,
    ExcessoTensionamentoCabos = 78,
    OrganizacaoFeixeCabosPoste = 79,

    // Outras constatações
    DescreverConstatacaoObservada = 80
}
