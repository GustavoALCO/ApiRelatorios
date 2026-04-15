using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IApiDocx
{
    Task<byte[]> emergencial(EmergencialDTO dto);
}
