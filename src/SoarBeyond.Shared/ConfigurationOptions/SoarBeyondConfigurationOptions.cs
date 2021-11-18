namespace SoarBeyond.Shared.ConfigurationOptions;

public class SoarBeyondConfigurationOptions
{
    public const string SectionKey = "SoarBeyond";
    public SoarBeyondPersistenceOptions Persistence { get; set; } = new();

    public string GetConnectionString()
    {
        var connString = $"Server={Persistence.Host};" +
                         $"Port={Persistence.Port};" +
                         $"Database={Persistence.Database};" +
                         $"Username={Persistence.Username};" +
                         $"Password={Persistence.Password};";

        return connString;
    }
}

public class SoarBeyondPersistenceOptions
{
    public bool UseInMemDb { get; set; }
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}