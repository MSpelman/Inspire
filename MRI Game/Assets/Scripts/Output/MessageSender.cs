using System.Net.Mail;

/*
 * Interface for wrapping the SmtpClient to allow for
 * unit testing.
 */
public interface MessageSender
{

	void Send(MailMessage mailMessage);

}
