namespace APIRelatorios.Application.Settings;

public class JWTSettings
{
    public string Key { get; set; }

    public string Issuer { get; set; }

    public List<string> Audience { get; set; } = new();

    public int ExpireDays { get; set; }
}
