using UnityEngine;
using System.Collections;
using Shared;

/* PowerUpController
 * This class is a script attached to the children of the PowerUp game object in the ScanView scene.
 * It is controls the behavior of an individual PowerUp object (e.g. a rotating star); these are
 * objects that move across the screen and generate points if the player contacts them.
 */
public class PowerUpController : MonoBehaviour {

	public float speed;  // How fast the game object moves across the screen; value set in the script component of game object

	// Holds the number of the last power-up spawned; used for determining consecutive power-ups
	private static int numberCounter = 0;

	private Rigidbody2D rb2d;  // RigidBody2D component needed for 2D physics
	private Log log;  // Reference to Log object the game should log events to
	private bool isTutorial;  // Needs to know if in tutorial mode when logging missed power-ups

	public int PowerUpNumber { get; private set; }  // The number assigned to this power-up

	/* Start is used for initialization */
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();  // Need a reference to the RigidBody2D so can apply a force to it
		isTutorial = !(GameSetup.GameMode);  // GameMode is false if in tutorial mode
		if (!isTutorial) {
			log = Log.GetInstance ();
		}

		if (this.gameObject.name.Contains ("Clone"))  // Only move spawned power-ups, not the template the copies are made from
		{
			numberCounter += 1;
			PowerUpNumber = numberCounter;

			Vector2 movement = new Vector2 (1, 0);  // Movement vector, only in X direction so Y == 0
			rb2d.AddForce (-speed * movement);  // Adds force to move game object; negative so goes right-to-left
		}
	}

	/* FixedUpdate is used for physics related updates; called once per frame
	 * In this case it adds a force to move power-up across the screen
	 */
	void FixedUpdate () 
	{

	}

	/* Update is used for non-physics related updates; called once per frame
	 * In this case it is rotating the power-ups and destroying them once they leave the left side of the
	 * screen.
	 */
	void Update () 
	{
		transform.Rotate (new Vector3 (0, 45, 0) * Time.deltaTime);  // Rotate the power-up

		// Log missed power-ups, then remove them
		if ((this.gameObject.transform.position.x <= -20.0) && (this.gameObject.name.Contains ("Clone")))
		{
			if (!isTutorial)
			{
				string eventDescription = "Missed Power-Up";
				GameEvent gameEvent = new GameEvent (eventDescription);
				log.LogEvent (gameEvent); //to record miss
			}
			Destroy (this.gameObject);
		}
	}
}
