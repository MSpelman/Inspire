using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Collections;

/*
 * Output is used to send an email with the game results to a specified address.
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
public class Output : IDisposable
{
	// MRI game email address
	string SendingAddress = "";  // Add the sending address for the emails

	// Email contents
	string EmailSubject = "Post-game report " + DateTime.Now.Date;
	string EmailBody = "";

	private MailMessage message;
	private MessageSender sender;

	// Creates an output using only the specified sender
	public Output (MessageSender sender)
	{
		this.message = new MailMessage ();
		this.sender = sender;
		FormatMessage ();
		AttachFiles ();
	}

	// Creates an output with specified message and sender
	// the message will have From, Subject, Body, To, and Attachment modified
	public Output (MailMessage message, MessageSender sender)
	{
		this.message = message;
		this.sender = sender;
		FormatMessage ();
		AttachFiles ();
	}

	// Emails the specified address with post game results
	public void SendTo(string emailAddress)
	{
		message.To.Add(new MailAddress(emailAddress));
		sender.Send (message);
	}

	private void FormatMessage()
	{
		message.From = new MailAddress (this.SendingAddress);
		message.Subject = this.EmailSubject;
		message.Body = this.EmailBody;
	}

	private void AttachFiles()
	{
		Log log = Log.GetInstance ();

		if (File.Exists (log.bellowsLogName)) 
		{
			System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(log.bellowsLogName);
			message.Attachments.Add (attachment);
		}
		if (File.Exists (log.calibrationLogName)) 
		{
			System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(log.calibrationLogName);
			message.Attachments.Add (attachment);
		}
		if (File.Exists (log.eventLogName)) 
		{
			System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(log.eventLogName);
			message.Attachments.Add (attachment);
		}
	}

	// Releases files
	public void Dispose(){
		message.Attachments.Dispose ();
		message.Dispose ();
	}
}
