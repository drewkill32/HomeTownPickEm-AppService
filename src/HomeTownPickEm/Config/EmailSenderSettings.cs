namespace HomeTownPickEm.Config;

public class EmailSenderSettings
{
    public const string SettingsKey = "Email";

    public string Key { get; set; }

    public string FromAddress { get; set; } = "adk@killion.me";

    public string Sender { get; set; }
}