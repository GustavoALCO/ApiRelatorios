namespace APIRelatorios.Application.Contracts.DTOs;

public record struct EvidenciaDocs
(
    string CID,

    string ALI,

    string DESC, 

    List<string> images
);