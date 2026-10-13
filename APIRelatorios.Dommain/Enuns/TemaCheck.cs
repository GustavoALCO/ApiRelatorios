namespace APIRelatorios.Dommain.Enuns;

public enum TemaCheck
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
    // 3. Adicione a descrição correspondente em Helpers/TemasCheckMapper.cs.
    //
    // Mantenha os valores existentes inalterados para preservar a integridade dos mapeamentos.
    // ===========================================================================================================

    SaidaDoAlimentador = 0,
    Vegetacao = 1,
    Postes = 2,
    Cruzetas = 3,
    Isoladores = 4,
    Condutores = 5,
    Seguranca = 6,
    Aterramento = 7,
    Transformadores = 8,
    ChavesReligadores = 9,
    ParaRaios = 10,
    EquipamentoInativo = 11,
    EquipamentoSemIdentificacao = 12,
    IluminacaoPublica = 13,
    SegurancaSinalizacao = 14,
    Compartilhamento = 15,
    OutrasConstatacoes = 16
}