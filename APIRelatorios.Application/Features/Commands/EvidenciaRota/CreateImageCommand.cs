using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct CreateImageCommand
(
int rotaID,

int fiscalId,

TemaFiscalizacao TemaFiscalizacao,

string? Identificacao,

string? Alimentador,

string Descricao,

string Base64,

string Endereco,

string Cep,

double Latitude,

double Longitude,

DateTime Horario
);
