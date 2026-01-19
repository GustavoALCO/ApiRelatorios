namespace ChatApplication.Application.Settings;

public class BlobSettings
{
    public required string ConnectionString { get; set; }

    public required string Container { get; set; }
}