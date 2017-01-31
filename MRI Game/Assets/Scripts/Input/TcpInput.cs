/*TCPInput is an Input class which attempts to receive bellows data from a server via a TCP connection.
 **/

using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TcpInput : BellowsInput
{
	// The server only sends position data, not the whole message.
	// Thus, we cannot use the constants provided by the BellowsInput class
	private const int MessageLength = 2;  // In bytes
	private const int BellowsPositionDataOffset = 0;

	private Thread GetterThread, LoggerThread, MainThread;
	private AutoResetEvent GetterSignaler, LoggerSignaler;
	private int ServerPort;
	private string ServerIP;
	private TcpClient Client;
	private NetworkStream Stream;
	private int[] CurrentBuffer, LogBuffer;
	private volatile int MostRecentData;

	public TcpInput (string address, int port){
		ServerIP = address;
		ServerPort = port;
		MainThread = Thread.CurrentThread;
		this.MostRecentData = 0;
		this.CurrentBuffer = new int[BellowsInput.BufferSize];
		this.LogBuffer = new int[BellowsInput.BufferSize];

		if (!this.Connect ()) {
			throw new Exception ("Cannot connect to server");
		}
		this.GetterSignaler = new AutoResetEvent(false);
		this.LoggerSignaler = new AutoResetEvent(false);
		this.GetterThread = new Thread(new ThreadStart(this.GetterThreadStart));
		this.LoggerThread = new Thread(new ThreadStart(this.LoggerThreadStart));
		this.GetterThread.Start();
		this.LoggerThread.Start();
	}

	public override int GetInput(){
		return this.MostRecentData;
	}

	/*
	Get DataThreadStart
	*/
	private void GetterThreadStart (){
		byte[] Buffer = new byte[MessageLength];
		int Index = 0;
		while(Client.Connected && MainThread.IsAlive){
			if(Index >= this.CurrentBuffer.Length){
				this.LoggerSignaler.WaitOne();//Wait for signal that logger is ready
				this.SwapBuffers();
				this.GetterSignaler.Set();//Signal to logger to log
				Index = 0;
			}
			if (this.Client.Available >= MessageLength) {
				try {
					int bytes_read = this.Stream.Read (Buffer, 0, Buffer.Length);
					if (bytes_read == -1) {
						continue;
					}
				} catch (Exception e) {
					Console.WriteLine ("Exception occured when reading from stream " + e.Message);
					break;
				}
				ushort Data = BitConverter.ToUInt16 (Buffer, BellowsPositionDataOffset);
				Data = (ushort)(4095 - Data);
				Debug.Log (Data);
				this.MostRecentData = (int)Data;
				this.CurrentBuffer [Index] = (int)Data;
				Index += 1;
			} else {
				Thread.Sleep (3);
			}
		}
		LoggerSignaler.WaitOne ();
		LoggerThread.Abort ();
	}

	/*
	LoggerThreadStart is the starting point for the Logger thread. This thread waits until the GetterThread has filled up
	its buffer with data, then swaps Getter's buffer with an empty buffer so that the filled buffer can be logged without causing
	latency.
	*/
	private void LoggerThreadStart (){
		while(MainThread.IsAlive){
			this.LoggerSignaler.Set();//Signal that logger is ready to run
			this.GetterSignaler.WaitOne();//wait for signal from getter
			this.Log(this.LogBuffer, this.LogBuffer.Length);	
			Array.Clear(this.LogBuffer, 0, this.LogBuffer.Length);
		}
	}
	private void Log(int[] Buffer, int Count){
		for (int n = 0; n < Count; n++) {
			Console.WriteLine (Buffer [n]);
		}
	}
	private void SwapBuffers(){
		int[] Temp = null;
		Temp = this.CurrentBuffer;
		this.CurrentBuffer = this.LogBuffer;
		this.LogBuffer = Temp;
	}
	private bool Connect(){
		try{
			this.Client = new TcpClient();
			this.Client.Connect(this.ServerIP, this.ServerPort);
			this.Stream = this.Client.GetStream ();
		}catch(Exception){
			return false;
		}
		return true;
	}
}