using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Shared;

/* GameOverController
 * This class is a script attached to an empty game object in the GameOver scene.
 * It contains the code that allows the user to return to the MainMenu scene.
 */
public class GameOverController : MonoBehaviour {

	public Text gameOverText;  // The UI text element used to hold the Game Over message

	private float startTime;  // Used to keep track of how long the tutorial has been running
	private string gameOverString;  // Holds the string to display on the Game Over screen

	/* Start is used for initialization */
	void Start () {
		startTime = Time.realtimeSinceStartup;  // Record start time
		gameOverText.text = "Game Over\n" + GameSetup.Score;
	}
	
	// Update is called once per frame
	void Update () {
		{
			// Return to Main Menu after 5 seconds
			if (Time.realtimeSinceStartup >= (startTime + 5)) 
			{
				SceneManager.LoadScene ("MainMenuScene");
			}
		}
	}
}
