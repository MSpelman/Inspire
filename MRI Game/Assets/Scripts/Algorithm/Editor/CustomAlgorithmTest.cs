using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Threading;
using System;

[TestFixture]
public class CustomAlgorithmTest {
	CustomAlgorithm customAlgorithm;
	Calibration calibration = new Calibration();

	[Test]
	public void ExpectedPositionTest()
	{
		calibration.Max = 6244;
		calibration.Min = 2048;

		float firstOffset = ((float) (calibration.Max - calibration.Min)) * (10f / 100f);
		float secondOffset = ((float) (calibration.Max - calibration.Min)) * (90f / 100f);
		int firstPos = calibration.Min + ((int)firstOffset);
		int secondPos = calibration.Min + ((int)secondOffset);

		for (int i = 0; i < 4; i++) {
			int position = customAlgorithm.ExpectedPosition();
			if ((i == 0) || (i == 2)) {
				Assert.AreEqual(firstPos, position, "ExpectedPositionTest(i=" + i.ToString() + ")");
			} else {
				Assert.AreEqual(secondPos, position, "ExpectedPositionTest(i=" + i.ToString() + ")");
			}
		}
	}

	[Test]
	public void SpawnPowerUpTest()
	{
		int position;

		// This resets the timer on SpawnPowerUp()
		while (customAlgorithm.SpawnPowerUp () < 1)
		{
			// Do nothing
		}

		// reseting the timer on SpawnPowerUp() uses up the first delay/position value in customalgorithm.txt
		// So we first check for the second pair (1^90), then make sure it rolls over to the first pair (2^10)
		for (int i = 0; i <= 12; i++)
		{
			position = customAlgorithm.SpawnPowerUp();
			if ((i == 4) || (i == 12)) {
				// ExpectedPositionTest validates the position is correct, this just needs to make sure 
				// position is > -1 when a pickup should spawn
				Assert.Greater (position, -1, "i = " + i.ToString());
			} else {
				Assert.AreEqual (-1, position, "i = " + i.ToString());
			}
			Thread.Sleep (250);
		}
	}

	[Test]
	public void ConstructorTest() {
		// Test case where file does not exist
		try {
			new CustomAlgorithm (calibration, "doesnotexist.txt");
			Assert.Fail();
		} catch (Exception) {

		}
		// Test case where file is empty
		try {
			new CustomAlgorithm (calibration, "empty.txt");
			Assert.Fail();
		} catch (Exception) {

		}
		// Test case where file has invalid data
		try {
			new CustomAlgorithm (calibration, "invalid.txt");
			Assert.Fail();
		} catch (Exception) {

		}
		// ExpectedPositionTest and SpawnPowerUpTest validate case where file has valid data
	}

	[SetUp]
	public void Initialize(){
		customAlgorithm = new CustomAlgorithm(calibration, "customalgorithm.txt");
	}
}