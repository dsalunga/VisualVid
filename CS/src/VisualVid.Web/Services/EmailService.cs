using System.Net;
using System.Net.Mail;

namespace VisualVid.Web.Services;

public class EmailSettings
{
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 25;
    public string FromAddress { get; set; } = "noreply@visualvid.com";
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool EnableSsl { get; set; }
}

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(to));

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort);
        if (!string.IsNullOrEmpty(_settings.UserName))
        {
            client.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);
        }
        client.EnableSsl = _settings.EnableSsl;

        await client.SendMailAsync(message);
    }
}
