using System.Net;
using System.Net.Mail;
using System.Text;

namespace backend.Services;

// https://blog.elmah.io/how-to-send-emails-from-csharp-net-the-definitive-tutorial/

public class EmailService
{
    private readonly string HOST = Environment.GetEnvironmentVariable("EmailHost");
    private readonly int PORT = int.Parse(Environment.GetEnvironmentVariable("EmailPort"));
    private readonly string ADDRESS = Environment.GetEnvironmentVariable("EmailAddress");
    private readonly string PASSWORD = Environment.GetEnvironmentVariable("EmailPassword");

    public EmailService(){}

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        using SmtpClient client = new SmtpClient(HOST, PORT)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(ADDRESS, PASSWORD)
        };

        MailMessage mail = new MailMessage(ADDRESS, to, subject, body)
        {
            IsBodyHtml = isHtml,
        };

        await client.SendMailAsync(mail);

        Console.WriteLine("Email enviado");
    }

    public async Task CreateEmailUser(string email, List<string> itemsFound)
    {
        string to = email;
        string subject = "Hay items en la tienda lol";
        StringBuilder body = new StringBuilder();
        body.AppendLine("<html><h1>Los siguientes items han vuelto a la tienda</h1><ul>");

        foreach (string item in itemsFound)
        {
            body.AppendLine($"<li>{item}</li>");
        }

        body.AppendLine("</ul></html>");
        await SendEmailAsync(to, subject, body.ToString(), true);
    }
}