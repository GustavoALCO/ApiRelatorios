using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class ConcessionariasMapper
{
    private static readonly Dictionary<Concessionarias, string> _map =
       new()
       {
            //Concessionárias 
            { Concessionarias.CPFL_Paulista, "Companhia Paulista de Força e Luz" },
            { Concessionarias.CPFL_Piratininga, "Companhia Piratininga de Força e Luz" },
            { Concessionarias.CPFL_Santa_Cruz, "Companhia Jaguari de Energia" },
            { Concessionarias.EDP, "EDP São Paulo Distribuição de Energia S.A." },
            { Concessionarias.ENEL, "Enel Distribuição São Paulo" },
            { Concessionarias.ENERGISA, "Energisa Sul-Sudeste – Distribuidora de Energia S.A." },
            { Concessionarias.ELEKTRO, "Neoenergia Elektro Redes S.A." },

            //Permissionárias 
            { Concessionarias.CETRIL, "Cooperativa de Eletrificação de Ibiúna e Região" },
            { Concessionarias.CERMC, "Cooperativa de Eletrificação e Desenvolvimento da Região de Mogi das Cruzes" },
            { Concessionarias.CERIM, "Cooperativa de Eletrificação e Desenvolvimento da Região de Itu-Mairinque" },
            { Concessionarias.CEDRAP, "Cooperativa de Eletrificação da Região do Alto Paraíba" },
            { Concessionarias.CEMIRIM, "Cooperativa de Eletrificação e Desenvolvimento da Região de Mogi Mirim" },
            { Concessionarias.CERPRO, "Cooperativa de Eletrificação Rural da Região de Promissão" },
            { Concessionarias.CERRP, "Cooperativa de Eletrificação e Desenvolvimento da Região de São José do Rio Preto – CERRP" },
            { Concessionarias.CERNHE, "Cooperativa de Eletrificação e Desenvolvimento Rural da Região de Novo Horizonte" },
            { Concessionarias.CERIS, "Cooperativa de Eletrificação da Região de Itapecerica da Serra" },
            { Concessionarias.CERIPA, "Cooperativa de Eletrificação Rural de Itaí, Paranapanema e Avaré Ltda." },
            { Concessionarias.COOPERLUZ_SP, "Cooperluz Cooperativa de Geração de Energia e Desenvolvimento" },
            { Concessionarias.CERES, "Ceres Tecnologia Ltda." },

            // Distribuidoras de transmissão
            { Concessionarias.ISA, "ISA CTEEP – Companhia de Transmissão de Energia Elétrica Paulista" }
       };

    public static string ToConc(this Concessionarias conc)
        => _map.TryGetValue(conc, out var desc)
            ? desc
            : throw new ArgumentNullException("Erro ao mapear Distribuidoras");
}
