using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using pizzashop_Services.interfaceService;

namespace pizzashop_Services.ImplementationService;

public class Email_Sevice:IEmail_Service
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Email_Sevice> _logger;

    public Email_Sevice(IConfiguration configuration, ILogger<Email_Sevice> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromAddress"])); // Fetch from config
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

        try
        {
            using (var emailClient = new SmtpClient())
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = Convert.ToInt32(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                await emailClient.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await emailClient.AuthenticateAsync(smtpUser, smtpPassword);
                await emailClient.SendAsync(emailToSend);
                await emailClient.DisconnectAsync(true);

                _logger.LogInformation($"Email sent to {email} with subject: {subject}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email to {email}. Exception: {ex.Message}");
            throw new InvalidOperationException("Error occurred while sending email.", ex);
        }
    }
}
