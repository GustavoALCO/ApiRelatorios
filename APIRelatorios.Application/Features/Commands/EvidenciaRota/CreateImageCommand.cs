using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct CreateImageCommand
(
int rotaID,

TemaFiscalizacao TemaFiscalizacao,

string Alimentador,

string Descricao,

string Base64,

string Endereco,

string Cep,

double Latitude,

double Longitude,

DateTime Horario
);
