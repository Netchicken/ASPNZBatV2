using System.Threading.Tasks;

namespace ASPNZBat.Business
{
    public interface IEmailSender
    {
        AuthMessageSenderOptions Options { get; }


        Task SendEmailAsync(string email, string subject, string message);
    }
}