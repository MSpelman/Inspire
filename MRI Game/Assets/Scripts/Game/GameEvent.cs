using UnityEngine;
using System.Collections;

/* GameEvent
 * This class defines an object type that encapsulates game events, such as collecting a power-up.
 * The objects are instatiated in the PlayerController and passed as a argument to the Log module,
 * where they are recorded.  Its only field is a description which is set via the constructor.
 */
public class GameEvent {

	public string description { get; private set; }

	public GameEvent(string description) {
		this.description = description;
	}
}
