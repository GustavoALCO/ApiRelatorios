using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IRelatorioDeIrregularidades
{
    Task<byte[]> BuildAsync(IEnumerable<DadosRelatorioDTO> dto);
}
