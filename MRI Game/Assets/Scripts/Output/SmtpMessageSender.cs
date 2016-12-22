using System.Net;
using System.Net.Mail;

/*
 * Wraps the SmtpClient to isolate usage and allow for testing.
 * SmtpMessageSender connects to the SmtpServer and sends
 * any messages passed.
 * 
 * Provides FromSystem static function to automatically set up credentials
 * specific to the Intr email address.
 * 
 * NOTE: This class in not currently in use by the game.  It is simply a template
 * that can be used if you want to enable automated emails.  Some things to keep in mind:
 * - If you use a public repository, you will want to create a config file to store
 * 		sensitive info (and code to read it in), such as the sending address, password, etc.  
 * 		This file should be ignored and not pushed to the repository.  Your IT people may also
 * 		want additional precautions to secure this information (i.e. they may not like having it
 * 		stored in a text file on the computer running the game).
 * - This was configured to use GMail; you may want to use something different
 * - GMail may require you to run some additional commands at the command line for certificates/credentialling
 */
public class SmtpMessageSender : MessageSender
{
	private SmtpClient smtpClient;

	// Sending email information
	private static string SendingAddress = "";  // Add your sending address (i.e. the address the emails come from)
	private static string SendingAddressPassword = "";  // Password for account sending the emails; see header for concerns

	// Host and port requirements
	private string host = "smtp.gmail.com";  // The template is set up for GMail, replace with your email provider
	private int port = 587;  // For GMail, other email providers may differ

	public SmtpMessageSender(ICredentialsByHost credentials)
	{
		smtpClient = new SmtpClient (host, port);
		smtpClient.EnableSsl = true;
		smtpClient.UseDefaultCredentials = false;
		smtpClient.Credentials = credentials;
	}

	// Returns a message sender specific to the MRI Game system
	public static SmtpMessageSender FromSystem()
	{
		ICredentialsByHost credentials = new NetworkCredential (SendingAddress, SendingAddressPassword) as ICredentialsByHost;
		return new SmtpMessageSender (credentials);
	}

	// Sends the message using the sender's credentials
	public void Send(MailMessage mailMessage)
	{
		smtpClient.Send (mailMessage);
	}

}
