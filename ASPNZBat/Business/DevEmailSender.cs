using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ASPNZBat.Business
{

    //MX Record Address:   igw5002.site4now.net
    //SMTP Host:   mail.stuffucanuse.com
    //SMTP Port:   25 or 8889 (if your isp blocks port 25)
    //SSL Ports:   SMTP: 465, POP3: 995, IMAP: 993
    //NZBAT@stuffucanuse.com 	VisionCollege 	NZBAT

    //https://stackoverflow.com/questions/51079522/asp-core-2-1-email-confirmation
    //https://dejanstojanovic.net/aspnet/2018/june/sending-email-in-aspnet-core-using-smtpclient-and-dependency-injection/


    //    "Email": {
    //"Smtp": {
    //"Host": "mail.stuffucanuse.com",
    //"Port": 8889,
    //"Username": "NZBAT@stuffucanuse.com",
    //"Password": "NZBAT1!"
    //}
    //}


    public class DevEmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        //from the IEmailSender

        //https://dotnetcoretutorials.com/2017/08/20/sending-email-net-core-2-0/

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("mail.stuffucanuse.com", 8889) //"yoursmtpserver")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("NZBAT@stuffucanuse.com", "NZBAT1!")
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("account-security-noreply@yourdomain.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            return client.SendMailAsync(mailMessage);
        }
    }
}
