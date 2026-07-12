using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Features.Commands.Amostra;

public record struct CreateAmostraCommand
(
    Guid RotaId,
    string SeqISA,
    string SeqBaseFisica,
    string VlrBase,
    string DescricaoTUC,
    string DescricaoTec,
    string ODIEngenharia,
    string Instalacao,
    string Endereco,
    string Municipio,
    double Latitude,
    double Longitude,
    string TUC1,
    string? TUC2,
    string? TUC3,
    string? TUC4,
    string? TUC5,
    string? TUC6,
    string? NumSerie,
    string? PosicaoOperativa,
    string? Equipamento
) : ICommand;
