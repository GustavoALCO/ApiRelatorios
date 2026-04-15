using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Dommain.Helpers;

public static class MounthMapper
{
    private static readonly Dictionary<Meses, string> _map =
      new()
      {
            { Meses.Janeiro, "Janeiro" },
            { Meses.Fevereiro, "Fevereiro" },
            { Meses.Marco, "Março" },
            { Meses.Abril, "Abril" },
            { Meses.Maio, "Maio" },
            { Meses.Junho, "Junho" },
            { Meses.Julho, "Julho" }, 
            { Meses.Agosto, "Agosto" },
            { Meses.Setembro, "Setembro" },
            { Meses.Outubro, "Outubro" },
            { Meses.Novembro, "Novembro" },
            { Meses.Dezembro, "Dezembro" },
      };

    public static string ToMes(this Meses mes)
        => _map.TryGetValue(mes, out var desc)
            ? desc
            : throw new Exception("");
}
