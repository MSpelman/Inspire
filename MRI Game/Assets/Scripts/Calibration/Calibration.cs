using UnityEngine;
using System.Collections;

/*
 * Calibration is a data object, storing the calibrated state.
 */
public class Calibration
{
	// Minimum position in the calibrated state
    public int Min { get; set; }
	// Maximum position in the calibrated state
    public int Max { get; set; }

    override public string ToString()
    {
        return "min: " + Min + "\tmax: " + Max;
    }


}