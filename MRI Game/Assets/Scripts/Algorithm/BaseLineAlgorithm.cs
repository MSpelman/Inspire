using UnityEngine;
using System.Collections;
using Shared;

public class BaseLineAlgorithm : Algorithm
{
	private Calibration calibration;

	public BaseLineAlgorithm(Calibration c) {
		calibration = c;
	}

    // create a subclass called baseline algorithm 
    // it implements the expected position function
    // return the calibration.min
    //static class called game setup, that has a field called calibration
	override public int ExpectedPosition()
    {
        return calibration.Min;
    }
}
