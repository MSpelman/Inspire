using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shared;

/* DynamicBackgroundSpawner
 * This class is a script attached to the the DynamicBackground game object in the ScanView scene.
 * It is controls the spawning of dynamic background objects (e.g. a swimming fish); these are
 * objects that move across the screen, but that the user cannot interact with.
 */
public class DynamicBackgroundSpawner : MonoBehaviour {
	// Array of dynamic background GameObjects. This allows it to spawn different types of dynamic background game objects
	// The array elements are set in the editor.  Select the DynamicBackground game object and find the script component in
	// the Inspector.  There should be a section called Dynamic Backgrounds which allows you to add game objects to the array.
	public GameObject[] dynamicBackgrounds;
	public static List<GameObject> spawnList;

	/* Start is used for initialization */
	void Start ()
	{
		spawnList = new List<GameObject> ();
		InvokeRepeating("Spawn", 2f, 1f);  // Calls the Spawn method every 1 second
	}
	
	/* Update is used for non-physics related updates; called once per frame */
	void Update () 
	{
	
	}

	/* Spawn is used for creating (spawning) new DynamicBackground game objects */
	void Spawn ()
	{
		// Randomly choose between available dynamic background game objects
		// 0 for swimmer (e.g. fish)
		// 1 for bottom dweller (e.g. seaweed or crab)
		// 3 for terrain feature (e.g. ruin or stalagtite)
		int number = Random.Range (0, 3);
		int index = (((GameSetup.Level - 1) * 3) + number);  // Appropriate index into array of dynamicBackgrounds
		GameObject newSpawn;

		// Instatiates a copy of the specifed game object at given location
        if (number != 2) 
        {
			newSpawn = (GameObject) Instantiate(dynamicBackgrounds[index], transform);
			spawnList.Add (newSpawn);
        } else
        {
			if(!(GameObject.Find("Ruin(Clone)") || GameObject.Find("Stalactite(Clone)")))
            {
				newSpawn = (GameObject) Instantiate(dynamicBackgrounds[index], transform);
				spawnList.Add (newSpawn);
            }
        }
	}
}