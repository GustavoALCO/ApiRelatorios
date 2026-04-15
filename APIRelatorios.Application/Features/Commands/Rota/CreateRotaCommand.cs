using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct CreateRotaCommand
(
    Guid? rotaId,

    string NomeRota, 

    Concessionarias Concessionarias,

    string Alimentador,

    List<int> Fiscais 

);
