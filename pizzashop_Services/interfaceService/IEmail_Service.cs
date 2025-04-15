namespace pizzashop_Services.interfaceService;

public interface IEmail_Service
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
