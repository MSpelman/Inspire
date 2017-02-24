using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameInput {

	/* Method that gets the input used by the game
	 * Used by all input methods, e.g., the actual bellows attached to the MRI,
	 * datafiles use in testing, simulated input, etc.
	 */
	int GetInput ();

}
