using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class InputModule : BellowsInput
{	
	private const int BUFFERSIZE = 64;
	private const string ConfigFilePath = "Assets/Scripts/Input/config.txt";

	private int ServerPort;
	private IPAddress ServerIP;
	private TcpClient TcpClient;
	private IPEndPoint ClientEndPoint;
	private IPEndPoint ServerEndPoint;

	private bool Connected;
	private double Time = 0;
	private int LogCount = 0;
	private Log Log;
	private int[] LogData;
    private Socket clientSocket;

	// This constructor initializes class variables using data stored in a file.
	public InputModule()
	{
	

		if (!File.Exists (ConfigFilePath))
		{
			Debug.Log("Unable to locate configuration file at path: " + ConfigFilePath + "Class not initialize");

		}
		else
		{
			try 
			{
                this.LogData = new int[BUFFERSIZE];
                this.Log = Log.GetInstance ();
                // this.Log.Start ();  // InputModule should NOT determine if log is running, other code will start/stop log

				using (StreamReader streamReader = new StreamReader(ConfigFilePath))
				{
					this.ServerIP = IPAddress.Parse (streamReader.ReadLine ());
					this.ServerPort = Int32.Parse (streamReader.ReadLine ());
				}
				this.TcpClient = new TcpClient ();
				this.TcpClient.Client.ReceiveTimeout = 100;
				this.TcpClient.Client.SendTimeout = 100;
//				this.ServerEndPoint = new IPEndPoint (ServerIP, this.ServerPort);
//				this.ClientEndPoint = new IPEndPoint (IPAddress.Any, 0);
				this.Connect ();
                this.clientSocket = TcpClient.Client;

			}
			catch (Exception e) 
			{
				Debug.Log("Unable to initialize Input class" + e.Message);
			}
		} 
	}

	/*
	 * Gets an actual input value
	 */
	public int GetInput() {
		return GetInput (false);
	}

	public int GetInput(bool simulated)
	{
		if (!simulated) 
		{
			byte[] Data = this.GetData ();
			if (Data == null)
			{
				return -1;
			}
			int ToReturn = Int32.Parse (System.Text.Encoding.ASCII.GetString (Data), 0);
			this.LogData [this.LogCount % BUFFERSIZE] = ToReturn;

			if (this.LogCount % BUFFERSIZE == BUFFERSIZE - 1)
			{
				this.Log.LogBellowsData (LogData);
			}
			this.LogCount++;

			return ToReturn;
		}
		else
		{

			this.Time += .05;
			return (int)(4096.0 * (1.0 - 0.5 * Math.Sin (Math.PI * 2.0 * (double)(Time % 8) / 8.0)));
		}
	}

	
	private byte[] GetData()
	{
		if (!this.Connected)
		{
			return null;
		}
		try
		{
			byte[] request = Encoding.ASCII.GetBytes("data");
            byte[] data = new byte[100];
			this.clientSocket.Send(request);
            this.clientSocket.Receive(data);

            return data;
		}
		catch(Exception e)
		{
			Debug.Log("Failed to get data from server" + e.Message);
		}
			return null;

	}
	private void Connect()
	{
		try
		{
//			byte[] message = Encoding.ASCII.GetBytes ("connect");
//			this.TcpClient.Send (message, message.Length, this.ServerEndPoint);
//			string reply = System.Text.Encoding.ASCII.GetString(TcpClient.Receive (ref this.ClientEndPoint));
			this.TcpClient.Connect (ServerIP,ServerPort);
			this.Connected = true;
		}
		catch(Exception e)
		{
			Debug.Log("Input class is unable to connect to server" + e.Message);
		}
	} 
}
