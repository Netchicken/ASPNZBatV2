using System.Threading.Tasks;

namespace ASPNZBat.Business
{
    public interface IEmailSender
    {
        AuthMessageSenderOptions Options { get; }

        Task Execute(string apiKey, string subject, string message, string email);
        Task SendEmailAsync(string email, string subject, string message);
    }
}