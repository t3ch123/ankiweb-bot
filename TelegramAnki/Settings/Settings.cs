namespace TelegramAnki.Settings;

public sealed class Settings
{
    public string PgConnectionString { get; set; } = null!;
    public string TgSecretToken { get; set; } = null!;
}
