using UnityEngine;
using System.Collections;
using Shared;

/*
 * Calibrator is a holder for the CalibrationController object.
 * Calibrator implements MonoBehavior for use in game objects, but
 * provides no additional behavior. Calibrator simply uses CalibrationController
 * for all of it's work.
 */
public class Calibrator : MonoBehaviour {

	private CalibrationController controller;

	// Used as a constructor
	void Start () {
		controller = new CalibrationController (GameSetup.Input);
	}
	
	// Updates each frame
	void Update () {
		controller.Update ();
	}

	// Returns the calibration stored in controller
	public Calibration GetCalibration() {
		return controller.Calibration;
	}
}
