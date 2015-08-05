using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using SendGrid;


namespace CorridaDePesso.Email
{
    public class SendGridMailer
    {
        private static string USER = "azure_62bd9278bd43990bcee755772cfce0be@azure.com";
        private static string PASS = "db0wfoap";
        
        protected static bool send(MailMessage message)
        {
            var credentials = new NetworkCredential(USER, PASS);
            var transportWeb = new Web(credentials);
            transportWeb.DeliverAsync(ParserMassegeSendGrid(message));
            return true;
        }

        private static ISendGrid ParserMassegeSendGrid(MailMessage message)
        {

            var sendGridMenssage = new SendGridMessage(); 
            sendGridMenssage.From = new MailAddress("noreplay@Corridadepeso.com.br");
            if (message.IsBodyHtml)
                sendGridMenssage.Html = message.Body;
            else
                sendGridMenssage.Text = message.Body;
            sendGridMenssage.Subject = message.Subject;
            foreach (var item in message.To)
                sendGridMenssage.AddTo(item.ToString());

            return sendGridMenssage;
            
        }   
    }
     
}
