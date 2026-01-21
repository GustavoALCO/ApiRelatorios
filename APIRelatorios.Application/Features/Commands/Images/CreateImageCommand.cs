namespace APIRelatorios.Application.Features.Commands.Images;

public readonly record struct CreateImageCommand
(
int rotaID,

string Descricao,

string Base64,

string Endereco,

int Cep,

double Latitude,

double Longitude,

DateTime Horario
);
