using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* SimInput
 * This class is used when we want simulated input.  This is helpful for testing modifications
 * to the game when you don't have access to an MRI/bellows and don't want to setup a server 
 * that reads from a text file.
 */
public class SimInput : IGameInput
{
	// Instance Variables
	private double time = 0;
	private Log log;

	// Constructor
	public SimInput()
	{
		log = Log.GetInstance ();  // Makes sure log is created, but doesn't actually start it
	}

	// Public Methods
	/* GetInput - Gets an input value for the game or calibrator to use
	 * Returns a simulated bellows position
	 */
	public int GetInput()
	{
		time += .05;
		return (int)(4096.0 * (1.0 - 0.5 * Math.Sin (Math.PI * 2.0 * (double)(time % 8) / 8.0)));
	}
}
