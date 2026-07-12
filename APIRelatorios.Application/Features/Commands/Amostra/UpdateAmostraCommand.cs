using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Amostra;

public record struct UpdateAmostraCommand
    (
        //Id de Amostra
        int Id,

        //Id Fiscal para buscar o nome futuralmente
        int IdFiscal,

        // Boleanos para a Atribuicao de valores
        bool tuc1,
        bool? tuc2,
        bool? tuc3,
        bool? tuc4,
        bool? tuc5,
        bool? tuc6,
        bool? numSerie,
        bool? posicaoOperativa,
        bool? equipamento,

        // Valores de Atribuicao livre 
        string dataFabricacao,
        string observacao,

        // fotos da amostra
        List<string> fotos,

        // Localizacao da amostra
        double latitude,
        double longitude

    ) : ICommand;

