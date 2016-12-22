using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Threading;

[TestFixture]
public class WaveFormAlgorithmTest {
	WaveFormAlgorithm waveFormAlgorithm;
	Algorithm algorithm;
	Calibration calibration = new Calibration();

	[Test]
	public void ExpectedPositionTest()
	{
		calibration.Max = 6244;
		calibration.Min = 2048;

		int bottomCount = 0;
		int topCount = 0;

		while ((topCount + bottomCount) < 4) 
		{
			int position = waveFormAlgorithm.ExpectedPosition();
			Assert.IsTrue ((position == calibration.Max) || (position == calibration.Min));

			if (position == calibration.Max)
			{
				topCount += 1;
			}
			else if (position == calibration.Min)
			{
				bottomCount += 1;
			}
		}
			
		Assert.AreEqual (1, topCount);
		Assert.AreEqual (3, bottomCount);
	}
	[Test]
	public void SpawnPowerUpTest()
	{
		int position;

		for (int i = 0; i < 4; i++)
		{
			position = algorithm.SpawnPowerUp();
			Assert.AreEqual (-1, position);
			Thread.Sleep (250);
		}
		Thread.Sleep (1);
		position = algorithm.SpawnPowerUp();
		Assert.IsTrue ((position == calibration.Max) || (position == calibration.Min));
	}
	[SetUp]
	public void Initialize(){
		waveFormAlgorithm = new WaveFormAlgorithm(calibration);
		algorithm = new WaveFormAlgorithm (calibration);
	}
}