using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class GameEventTest {

	GameEvent gameEvent;

	string description = "This is a description";

	[SetUp]
	public void Initialize()
	{
		gameEvent = new GameEvent (description);
	}

	[Test]
	public void EventStoresDescription()
	{
		Assert.AreEqual (description, gameEvent.description);
	}
}
