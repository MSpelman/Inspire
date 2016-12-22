using UnityEngine;
using System.Collections;
using Shared;

/* WaveFormAlgorithm
 * This class implements the wave form game mode, which encourages the patient to breath rythmically.
 */
public class WaveFormAlgorithm : Algorithm
{
	private Calibration calibration;
	private int bottomCount;

	public WaveFormAlgorithm(Calibration c)
	{
		bottomCount = 0;
		calibration = c;
	}

	/* ExpectedPosition calculates the position where the power-up should spawn
	 * 
	 * return The bellows position for the new power-up
	 */
	override public int ExpectedPosition()
	{
		int result;

		if (bottomCount < 3)
		{
			result = calibration.Min;
			bottomCount++;
		}
		else
		{
			result = calibration.Max;
			bottomCount = 0;
		}

		return result;
	}
}