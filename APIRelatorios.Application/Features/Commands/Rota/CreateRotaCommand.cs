using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Application.Features.Commands.Rota;

public readonly record struct CreateRotaCommand
(
    string? NomeRota, 

    ICollection<int> Fiscais 

);
