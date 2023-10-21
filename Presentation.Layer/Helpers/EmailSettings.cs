using Data.Access.Layer.Models;
using System.Net;
using System.Net.Mail;

namespace Presentation.Layer.Helpers
{
	public static class EmailSettings
	{
		public static void sendEmail(Email email)
		{
			//SmtpClient =>> allow applicatin to send email by using SMTP protocol
			var client = new SmtpClient("smtp.gmail.com", 587); //take 2 parameter hostname[for company that we make project],port [email server]
			client.EnableSsl = true; //make email encrypted
			client.Credentials = new NetworkCredential("eng.mostafagawish@gmail.com", "whrqneouflotdnyw");//From[Sender info] {Email,password}
			client.Send("eng.mostafagawish@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
