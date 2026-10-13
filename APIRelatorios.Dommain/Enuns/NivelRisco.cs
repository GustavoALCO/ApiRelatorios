namespace APIRelatorios.Domain.Enuns;

public enum NivelRisco
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
    //
    // Mantenha os valores existentes inalterados para preservar a integridade dos mapeamentos.
    // ===========================================================================================================

    Normal = 0,

    Alta = 1,

    Emergencial = 2,
}