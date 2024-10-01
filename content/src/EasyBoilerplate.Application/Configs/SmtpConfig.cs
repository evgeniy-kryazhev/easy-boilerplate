namespace ExCraft.Host.Configs;

public class SmtpConfig
{
    public required string Host { get; set; } = "smtp.timeweb.ru";
    public required int Port { get; set; } = 465;
    public required string Username { get; set; }
    public required string Password { get; set; }
}