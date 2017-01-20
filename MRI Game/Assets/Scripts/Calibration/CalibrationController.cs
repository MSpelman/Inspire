using UnityEngine;
using System.Collections;

/*
 * Calibration controller handles processing input values
 * to determine the calibration state.
 */
public class CalibrationController
{

	private BellowsInput input;

	// Provides access to the calibration created.
    public Calibration Calibration { get; private set; }

    public CalibrationController(BellowsInput input)
    {
        this.input = input;
		Calibration = new Calibration ();
        Calibration.Max = int.MinValue;
        Calibration.Min = int.MaxValue;
    }

	/*
	 * Updates the calibration state with the next input value.
	 */
    public void Update()
    {
        int val = input.GetInput();  // Remove true when use actual server
        if (val > Calibration.Max)
        {
            Calibration.Max = val;
        }
        if (val < Calibration.Min)
        {
            Calibration.Min = val;
        }
    }
}