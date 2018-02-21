using UnityEngine;
using System.Collections;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;

/* CustomAlgorithm
 * This class implements a customizable game mode, which can be changed simply by editing a text file.
 */
public class CustomAlgorithm : Algorithm
{
	private Calibration calibration;
	static DateTime time = DateTime.Now;
	private int currentIdx;
	List<int> delayList;
	List<int> positionList;

	public CustomAlgorithm(Calibration c, string path)
	{
		calibration = c;
		delayList = new List<int> ();
		positionList = new List<int> ();
		ReadCustomAlgorithmFile (path);
		currentIdx = 0;
	}

	/* SpawnPowerUp determines if a power up should be spawned, and if so
	 * calls ExpectedPosition to return where it should spawn
	 * 
	 * return The bellows position where the pick-up should spawn, or -1 if a pick-up should not spawn yet
	 */
	public override int SpawnPowerUp()
	{
		// Get delay
		int delay = delayList [currentIdx];

		// Check time,
		// If not time to spawn power up, return -1
		// Otherwise, call gamemode method for position
		DateTime currTime = DateTime.Now;
		TimeSpan duration = currTime - time;
		if (duration.Seconds < delay)
		{
			return -1;
		}
		else {
			time = DateTime.Now;//reset the global variable
			int position = ExpectedPosition();
			return position;
		}
	}

	/* ExpectedPosition calculates the position where the power-up should spawn
	 * 
	 * return The bellows position for the new power-up
	 */
	override public int ExpectedPosition()
	{
		float position = (float) positionList [currentIdx];
		float offset = ((float) (calibration.Max - calibration.Min)) * (position / 100f);
		currentIdx++;
		if (currentIdx >= delayList.Count) currentIdx = 0;
		return (calibration.Min + ((int) offset));
	}

	private void ReadCustomAlgorithmFile(string path) {
		string algoString;

		// verify file exists
		if (!File.Exists (path))
		{
			Console.Write("Custom algorithm file is missing");
			throw new Exception ("Custom Algorithm file missing");
		} else {
			// try opening and reading in file
			try {	
				using (StreamReader reader = new StreamReader (File.Open (path, FileMode.Open))) {
					algoString = reader.ReadLine ().Trim();
				}
			}
			catch (Exception e)
			{
				Console.Write("Problem reading " + path + ": " + e.Message);
				throw new Exception ("Problem reading custom algorithm file");
			}
		}

		// make sure file actually contained something
		if (algoString.Length < 1) {
			Console.Write (path + "is empty");
			throw new Exception (path + " is empty.");
		}

		// parse the values out of the string read in from the file
		var lines = algoString.Split ('|');
		foreach (string line in lines) {
			var fields = line.Split ('^');
			int delay = Int32.Parse (fields [0]);
			int position = Int32.Parse (fields [1]);
			// check if valid values
			if ((delay >= 0) && (delay <= 15) && (position >= 0) && (position <= 100)) {
				delayList.Add (delay);
				positionList.Add (position);
			}
		}

		// make sure file actually had some valid data
		if (delayList.Count == 0) {
			Console.Write ("No valid data in " + path);
			throw new Exception ("No valid data in " + path);
		}
	}
}