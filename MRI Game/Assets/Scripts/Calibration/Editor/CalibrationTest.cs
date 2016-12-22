using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CalibrationTest
{
    int MIN = 132;
    int MAX = 589;

    [Test]
    public void CalibrationFormatsToStringCorrectly()
    {
        Calibration calibration = new Calibration();
        calibration.Min = MIN;
        calibration.Max = MAX;

        var expected = "min: " + MIN + "\tmax: " + MAX;

        Assert.AreEqual(expected, calibration.ToString());
    }
}