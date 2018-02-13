using UnityEngine;
using System;
using System.Collections;
using System.IO;

/* The use of 'bellows' in this class is a misnomer; it is actually referring to game data
 * gameData = 4095 - bellowsData
 * 
 * The name of the log file was updated, as this is something users see, but the code still needs to be
 * re-written to reflect this.
 */
public class Log
{

	private static volatile Log instance;

	private Boolean running;

	// Accessors to the files written to by log.
	public string bellowsLogName { get; private set; }

	public string eventLogName { get; private set; }

	public string calibrationLogName { get; private set; }

	private Log ()
	{
		bellowsLogName = "";
		eventLogName = "";
		calibrationLogName = "";
		running = false;
	}

	public static Log GetInstance ()
	{
		if (instance == null) {
			// TODO: Make threadsafe
			instance = new Log ();
		}
		return instance;
	}

	/*
	 * Enables logging to files.
	 * Creates and timestamps new files.
	 * isCalibration - true if logging calibration data, false if logging game events
	 */
	public void Start (bool isCalibration, string techName, string techEmail)
	{
		running = true;
		// RemoveFiles ();
		TimestampFilenames ();
		CreateFiles (isCalibration, techName, techEmail);
	}

	/*
	 * Overloaded versions of above method
	 */
	public void Start (bool isCalibration)
	{
		Start (isCalibration, PlayerPrefs.GetString("techName"), PlayerPrefs.GetString("techEmail"));
	}

	public void Start ()
	{
		Start (false, PlayerPrefs.GetString("techName"), PlayerPrefs.GetString("techEmail"));
	}

	/*
	 * Disables logging to files.
	 */
	public void Stop ()
	{
		running = false;
	}

	public void LogBellowsData (int[] data)
	{
		if (!running) {
			return;
		}
		using (var writer = File.AppendText (bellowsLogName)) {
			foreach (var value in data) {
				writer.WriteLine (value);
			}
		}
	}

	public void LogEvent (GameEvent gameEvent)
	{
		if (!running) {
			return;
		}
		using (var writer = File.AppendText (eventLogName)) {
			writer.WriteLine (Timestamp () + "\t" + gameEvent.description);
		}
	}

	public void LogCalibration (Calibration calibration)
	{
		if (!running) {
			return;
		}
		using (var writer = File.AppendText (calibrationLogName)) {
			writer.WriteLine (Timestamp () + "\t" + calibration);
		}
	}

	/* This method is not currently being used, but was not completely removed
	 * as it may be needed in the future */
	private void RemoveFiles ()
	{
		if (File.Exists (bellowsLogName)) {
			File.Delete (bellowsLogName);
		}
		if (File.Exists (eventLogName)) {
			File.Delete (eventLogName);
		}
		if (File.Exists (calibrationLogName)) {
			File.Delete (calibrationLogName);
		}
	}

	private void TimestampFilenames ()
	{
		bellowsLogName = "gamedatalog_" + Timestamp ();
		eventLogName = "eventlog_" + Timestamp ();
		calibrationLogName = "calibrationlog_" + Timestamp ();
	}

	private void CreateFiles (bool isCalibration, string techName, string techEmail)
	{
		try {
			if (isCalibration)
			{
				File.Create (calibrationLogName).Close ();
				using (var writer = File.AppendText (calibrationLogName)) {
					writer.WriteLine (techName + "(" + techEmail + ")");
				}
			} else {
				File.Create (eventLogName).Close ();
				using (var writer = File.AppendText (eventLogName)) {
					writer.WriteLine (techName + "(" + techEmail + ")");
				}
			}
			File.Create (bellowsLogName).Close ();
		} catch (Exception e) {
			Console.Write ("Unable to create file " + e);
		}
		
	}

	private string Timestamp ()
	{
		var time = DateTime.Now.ToString ("u");
		int os = (int)Environment.OSVersion.Platform;
		if (os != 4 && os != 6 && os != 128) {
			time = time.Replace (':', '.');
		}
		return time;
	}
}
