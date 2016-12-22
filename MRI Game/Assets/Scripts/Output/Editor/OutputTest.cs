using System.Net.Mail;
using System.Net;
using System;
using System.IO;
using NUnit.Framework;
using NSubstitute;

public class OutputTest {
    // Object under test
    Output output;

    // Mocked dependencies
    MailMessage mockMessage;
    MessageSender mockSender;

    // Actual instance is required to check file attachments
    Log log;

    // Expected components of the email
    string subject = "Post-game report " + DateTime.Now.Date;
    string body = "";
    string techEmail = "";  // Add a fake tech email address to sent message to
    string fromAddress = "";  // Add your sending email address
    MailAddress techAddress;

    [SetUp]
    public void Setup()
    {
        log = Log.GetInstance();
        techAddress = new MailAddress(techEmail);
        mockMessage = Substitute.For<MailMessage>();
        mockSender = Substitute.For<MessageSender>();

        output = new Output(mockMessage, mockSender);
    }

    [Test]
    public void OutputCorrectlyFormatsMessageAndSends()
    {
        output.SendTo(techEmail);

        // Message is formatted correctly
        Assert.AreEqual(1, mockMessage.To.Count);
        Assert.IsTrue(mockMessage.To.Contains(techAddress));
        Assert.AreEqual(fromAddress, mockMessage.From.Address);
        Assert.AreEqual(subject, mockMessage.Subject);
        Assert.AreEqual(body, mockMessage.Body);

        // No files are attached
        Assert.AreEqual(0, mockMessage.Attachments.Count);

        // Message is sent to tech's email
        mockSender.Received().Send(mockMessage);
    }
    [Test]
    public void OutputCorrectlyCreatesOneArgConstructor()
    {
        try {
            output = new Output(mockSender);
        }catch(Exception e)
        {
            //Testing to make sure nothing fails during construction
            Assert.Fail("Expected success with one arg constructor");
        }

    }
	[Test]
	public void OutputCorrectlyAttachesFilesToMessageAndSends()
	{
		// Create files
		log.Start ();
		log.Stop ();

		// Create new output to attach the files
		output = new Output (mockMessage, mockSender);
		output.SendTo (techEmail);

		Assert.AreEqual(3, mockMessage.Attachments.Count);
	}

	[Test]
	public void OutputDoesNotAttachNonExistentFiles()
	{
		// Log is not started, so no files are created
		output.SendTo (techEmail);

		Assert.AreEqual(0, mockMessage.Attachments.Count);
	}

	[TearDown]
	public void TearDown()
	{
		output.Dispose ();
		log.Stop ();
		CleanFiles ();
	}

	private void CleanFiles()
	{
		if (File.Exists(log.bellowsLogName))
		{
			File.Delete (log.bellowsLogName);
		}
		if (File.Exists(log.calibrationLogName))
		{
			File.Delete (log.calibrationLogName);
		}
		if (File.Exists(log.eventLogName))
		{
			File.Delete (log.eventLogName);
		}
	}
}
