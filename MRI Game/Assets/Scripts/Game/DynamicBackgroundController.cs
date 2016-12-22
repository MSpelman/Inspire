using UnityEngine;
using System.Collections;

/* DynamicBackgroundController
 * This class is a script attached to the children of the DynamicBackground game object in the ScanView scene.
 * It is controls the behavior of an individual dynamic background object (e.g. a swimming fish); these are
 * objects that move across the screen, but that the user cannot interact with.
 */
public class DynamicBackgroundController : MonoBehaviour {
	
	public float speed;  // How fast the game object moves across the screen; value set in the script component of game object

	private Rigidbody2D rb2d;  // RigidBody2D component needed for 2D physics

	/* Start is used for initialization */
	void Start () 
	{
		if (this.gameObject.name.Contains("Clone"))
		{
			// Sets the spawning position of the dynamic background object.  An X value of 10.5 spawns the game object
			// just off the right side of the screen.  The Y position is a randomly selected coordinate that is within
			// the water.
			if (this.gameObject.name.Contains ("Fish") || this.gameObject.name.Contains ("CaveFish")) {
				this.gameObject.transform.position = new Vector3 (10.5f, Random.Range (-3f, 2.75f));
			} else if (this.gameObject.name.Contains("Seaweed")) {
				this.gameObject.transform.position = new Vector3 (10.5f, -4.25f);
			} else if (this.gameObject.name.Contains("Ruin")) {
				this.gameObject.transform.position = new Vector3 (10.5f, -1.45f);
			} else if (this.gameObject.name.Contains("Stalactite")) {
				this.gameObject.transform.position = new Vector3 (10.5f, 4.3f);
			} else if (this.gameObject.name.Contains("Rock")) {
				this.gameObject.transform.position = new Vector3 (10.5f, -4.50f);
			}

			rb2d = GetComponent<Rigidbody2D> ();  // Need a reference to the RigidBody2D so can apply a force to it
  	     	Vector2 movement = new Vector2(1, 0);  // Movement vector, only in X direction so Y == 0
   	    	rb2d.AddForce(-speed * movement);  // Adds force to move game object; negative so goes right-to-left
		}
    }

	/* FixedUpdate is used for physics related updates; called once per frame
	 * In this case it adds a force to move the background object across the screen
	 */
	void FixedUpdate() 
	{
		
	}

	/* Update is used for non-physics related updates; called once per frame
	 * In this case it is destroying the spawned background objects once they leave the left side of the
	 * screen.
	 */
	void Update ()
	{
		if ((this.gameObject.transform.position.x <= -22.0) && (this.gameObject.name.Contains ("Clone")))
		{
			DynamicBackgroundSpawner.spawnList.Remove (this.gameObject);
			Destroy (this.gameObject);
		}
	}
}
