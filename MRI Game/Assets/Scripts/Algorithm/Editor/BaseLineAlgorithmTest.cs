using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Threading;

[TestFixture]
public class BaseLineAlgorithmTest {
	BaseLineAlgorithm baseLine;
	Algorithm algo;
	Calibration c = new Calibration();
	[Test]
	public void ExpectedPositionTest()
	{
		c.Min = 200;
		int a = baseLine.ExpectedPosition();
		Assert.AreEqual (c.Min, a);
	}
	[Test]
	public void SpawnPowerUpTest()
	{
		//int milliseconds = 500;
		//int a = algo.SpawnPowerUp();
		//Assert.AreEqual (-1, a);
		//Thread.Sleep (milliseconds);
		//int b = algo.SpawnPowerUp();
		//Assert.AreEqual (-1, b);
		//Thread.Sleep (milliseconds);
		//int c = algo.SpawnPowerUp();
		//Assert.AreEqual (-1, c);
		//Thread.Sleep (milliseconds);
		//int d = algo.SpawnPowerUp();
		//Assert.AreEqual (-1, d);
		//Thread.Sleep (501);
		//int e = algo.SpawnPowerUp();
		//Assert.AreEqual (200, e);
		for (int i = 0; i < 4; i++) {
			int a = algo.SpawnPowerUp();
			Assert.AreEqual (-1, a);
			Thread.Sleep (250);
		}
		Thread.Sleep (1);
		int b = algo.SpawnPowerUp();
		Assert.AreEqual (200, b);
	}
	[SetUp]
	public void Initialize(){
		baseLine = new BaseLineAlgorithm(c);
		algo = new BaseLineAlgorithm (c);
	}
}
