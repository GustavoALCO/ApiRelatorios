namespace APIRelatorios.Dommain.Enuns;

public enum Concessionarias
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
    // 2. Adicione a descrição correspondente em Helpers/ConcessionariasMapper.cs.
    //
    // Mantenha os valores existentes inalterados para preservar a integridade dos mapeamentos.
    // ===========================================================================================================

    // Distribuidoras
    CPFL_Paulista = 0,
    CPFL_Piratininga = 1,
    CPFL_Santa_Cruz = 2,
    EDP = 3,
    ENEL = 4,
    ENERGISA = 5,
    ELEKTRO = 6,

    // Permissionárias
    CETRIL = 7,
    CERMC = 8,
    CERIM = 9,
    CEDRAP = 10,
    CEMIRIM = 11,
    CERPRO = 12,
    CERRP = 13,
    CERNHE = 14,
    CERIS = 15,
    CERIPA = 16,
    COOPERLUZ_SP = 17,
    CERES = 18,

    // Distribuidoras de transmissão
    ISA = 19,
}
