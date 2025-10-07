using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helpers
{
    public class EmailSetting
    {
        public static bool SendEmail(Email email) 
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("maimustafa614@gmail.com", "zswuxzrtnwmxnsep");
                client.Send("maimustafa614@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }  
}
