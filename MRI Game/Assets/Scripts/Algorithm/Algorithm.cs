using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public abstract class Algorithm
{
    static DateTime time = DateTime.Now; // global variable
                                         //public DateTime Time {get; set;}
                                         // Use this for initialization
    public int SpawnPowerUp()
    {
        // Check time,
        // If not time to spawn power up, return -1
        // Otherwise, call gamemode method for position
        DateTime currTime = DateTime.Now;
        TimeSpan duration = currTime - time;
        if (duration.Seconds < 1)
        {
            return -1;
        }
        else {
            time = DateTime.Now;//reset the global variable
            return ExpectedPosition();
        }
    }
    public abstract int ExpectedPosition();
}
