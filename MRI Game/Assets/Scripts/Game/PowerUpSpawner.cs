using UnityEngine;
using System.Collections;
using Shared;
using System;

/* PowerUpSpawner
 * This class is a script attached to the the PowerUp game object in the ScanView scene.
 * It is controls the spawning of power-up objects (e.g. a rotating star); these are
 * objects that move across the screen and generate points if the player contacts them.
 */
public class PowerUpSpawner : MonoBehaviour {
	// Array of power-up GameObjects. This allows it to spawn different types of power-up game objects
	// The array elements are set in the editor.  Select the PowerUp game object and find the script component in
	// the Inspector.  There should be a section called Power Ups which allows you to add game objects to the array
	public GameObject[] powerUps;
	private Calibration calibration;  // Calibration object which holds the calibration data the game uses
	private Algorithm algorithm;  // Algorithm object that is used to determine when and where power-ups spawn
	private double m;  // Holds data used in normalization calculation
	private double b;  // Holds data used in normalization calculation

	/* Start is used for initialization */
	void Start ()
	{
		calibration = GameSetup.CalibrationController.Calibration;
		SetNormalization (calibration.Max, calibration.Min);  // Map bellows positions to power-up positions in game
		algorithm = GetAlgorithm();
		InvokeRepeating("Spawn", 0.5f, 0.5f);  // Calls the Spawn method every 1 second
	}
	
	/* Update is used for non-physics related updates; called once per frame */
	void Update () 
	{

	}

	/* Spawn is used for creating (spawning) new PowerUp game objects */
	void Spawn ()
	{
		int position;  // A bellows position
		float vertical;  // The vertical position of the player on the screen
		position = algorithm.SpawnPowerUp ();
		if (position > 0)
		{
			// Guard against inappropriate position
			if (position < calibration.Min)
				position = calibration.Min;
			if (position > calibration.Max)
				position = calibration.Max;

			// Sets the spawning position of the power-up object.  An X value of 10.5 spawns the game object
			// just off the right side of the screen.  The Y position is based on the return value of SpawnPowerUp()
			vertical = GetVertical(position);  // Translate the bellows position to the power-up's vertical position on the screen
			Instantiate (powerUps [0], transform);  // Instatiates a copy of the specifed game object at given location
			// Sets position of power-up just spawned
			transform.GetChild(transform.childCount - 1).position = new Vector3 (10.5f, vertical);
		}
	}
		
	/* SetNormalization normalizes power-up position based on calibration data
	 * 
	 * calMax  The maximum calibration value
	 * calMin  The minimum calibration value
	 */
	private void SetNormalization(int calMax, int calMin)
	{
		// pad calibration to force pick-ups towards middle of player's range
		// this makes it so the player can still contact them even if there is "calibration drift"
		double diff = (double) (calMax - calMin);
		double pad = diff * 0.10;  // pad by 10.0%
		double max = (double)calMax + pad;
		double min = (double)calMin - pad;
		// This conversion is based on the equation for a line
		// y = (m * x) + b
		// y is the power-up's position
		// m is the slope of the line
		// x is the bellows postion
		// b is the intercept
		// max gets mapped to the highest player position (2.65767)
		// min gets mapped to the lowest player position (-3)
		// All other positions fall somewhere on the line between the two
		m = 5.65767d / (max - min);
		b = 2.65767d - (m * max);
	}

	/* GetVertical calculates the power-up's vertical position in the game based on the
	 * given bellows position
	 * 
	 * bellowsPosition  The bellows position
	 * 
	 * return The power-up's position in the game based on the given bellow position
	 */
	private float GetVertical(int bellowsPosition)
	{
		return (float)(m * (double) bellowsPosition + b);  // Uses line equation set up in SetNormalization()
	}

	/* GetAlgorithm looks up the selected algorithm id and instantiates an algorithm object of the correct type
	 * 
	 * return an Algorithm object created based on the correct subclass
	 */
	private Algorithm GetAlgorithm()
	{
		// Right now there is only on algorithm, but in the future this could be a select/case
		if (GameSetup.AlgorithmID == 0) {
			return new BaseLineAlgorithm (calibration);
		} else if (GameSetup.AlgorithmID == 1) {
			return new WaveFormAlgorithm (calibration);
		} else if (GameSetup.AlgorithmID == 2) {
			try {
				return new CustomAlgorithm (calibration, "customalgorithm.txt");
			} catch (Exception) {
				// CustomAlgorithm constructor could throw exceptions; if so, just use BaseLine
				return new BaseLineAlgorithm (calibration);
			}
		} else {
			return new BaseLineAlgorithm (calibration);
		}
	}
}
