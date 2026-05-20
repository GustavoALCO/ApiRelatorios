namespace APIRelatorios.Dommain.Enuns;

public enum SubTemaAlimentadores
{
    // ======================
    // ======================
    // ENUNS DE ALIMENTADORES
    // ======================
    // ======================

    // ==============
    // TEMAS DE POSTE
    // ==============

    //Estado fisico (trincas, apodrecimento, corrosao, empenamento)
    EstadoFisico = 0,
    // Estabilidade e alinhamento
    EstabilidadeEAlinhamento = 1,
    // Identificacao legivel
    IdentificacaoLegivel = 2,
    //Sinais de colisao/impacto
    SinaisdeColisao_Impacto = 3,
    //Condicões da base (exposicao, erosao)
    CondicoesDaBase = 4,

    // =========================
    // TEMAS DE COMPARTILHAMENTO
    // =========================

    //Posicionamento correto dos cabos abaixo da rede elétrica
    PosicionamentoCorretoDosCabos = 5,
    //Afastamento Minimo
    AfastamentoMinimo = 6,
    //Ausência de cabos soltos, rompidos ou apoiados em ferragens elétricas
    AusenciaDeCabosSoltos = 7,
    //Fixacao adequada de caixas
    FixacaoAdequada = 8,
    //Identificacao do responsável pelo cabo
    IdentificacaoResponsavel = 9,
    //Indicios de ocupacao clandestina
    OcupacaoClandestina = 10,
    //Organizacao do feixe de cabos no poste
    OrganizacaoDoFeixeDeCabosNoPoste = 11,


    // =============================
    // TEMAS DE ESTRUTURAS/FERRAGENS
    // =============================

    //Integridade de cruzetas, suportes
    IntegridadeCruzetasSuportes = 12,
    //Aperto de porcas e parafusos
    ApertoDePorcasEParafusos = 13,
    //Corrosao ou desgaste
    CorrosaoDesgaste = 14,

    // ===================
    // TEMAS DE ISOLADORES
    // ===================

    //Trincas, quebras ou fuligem
    TrincasQuebras = 15,
    //Fixacao na cruzeta
    FixacaoCruzeta = 16,

    // ===================
    // TEMAS DE CONDUTORES
    // ===================

    //Estado fisico (desgaste, emendas irregulares)
    EstadoFisicoDesgaste = 17,
    //Tensao mecânica adequada
    TensaoMecanicaAdequada = 18,

    //Afastamento de árvores/estruturas
    AfastamentoEstruturas = 19,

    // ====================
    // TEMAS DE ATERRAMENTO
    // ====================

    //Conexões corretas e continuas
    PresencaEstado = 20,
    //Conexões corretas e continuas
    ConexoesCorretas = 21,

    // ========================
    // TEMAS DE TRANSFORMADORES
    // ========================

    //Vazamentos, corrosao, pintura
    VazamentoCorrosao = 22,
    //Nivel de óleo/visores
    NivelOleo = 23,
    //Estado das buchas
    EstadoBuchas = 24,
    //Para-raios integros
    ParaRaios = 25,

    // ===========================
    // TEMAS DE Chaves/Religadores
    // ===========================

    //Integridade fisica, ausência de fuligem
    IntegridadeFisica = 26,
    //Contatos e manobrabilidade
    ContatosManobrabilidade = 27,
    //Sinalizacao de posicao
    SinalizacaoPosicao = 28,

    // ===================
    // TEMAS DE Para-raios
    // ===================

    //Fixacao correta
    FixacaoCorreta = 29,

    //Trincas, queimaduras, fuligem
    TrincasQueimaduras = 30,
    //Conexao de aterramento
    ConexaoAterramento = 31,
    
    // ============================
    // TEMAS DE Iluminacao Pública
    // ============================

    //Estado da luminária
    EstadoLuminaria = 32,
    //Funcionamento da lâmpada
    FuncionamentoLampada = 33,
    //Fotocélula em operacao
    FotocelulaOperacao = 34,
    //Fiacao exposta ou mal fixada
    FiacaoExposta = 35,

    // ===================
    // TEMAS DE Vegetacao
    // ===================

    //Galhos próximos à rede elétrica"
    GalhosProximos = 36,
    //Risco de queda de árvores
    RiscoDeQueda = 37,

    // ===================
    // TEMAS DE Seguranca/Sinalizacao
    // ===================

    //Placas de advertência visiveis
    PlacasAdvertenciaVisiveis = 38,
    //Barreiras de protecao adequadas
    BarreirasAdequadas = 39,
    //Conformidade com normas (NR-10, concessionária)
    ConformidadeNormas = 40,

    // ===================
    // TEMAS DE OUTROS
    // ===================

    //Outros
    Outros = 41
}
