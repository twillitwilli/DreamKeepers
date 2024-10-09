using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;
using SoT.Classes;

public class DKTime : MonoSingleton<DKTime>
{
    [SerializeField]
    Rotation rotationController;

    public enum TimeOfDay
    {
        morning,
        afternoon,
        evening,
        dusk,
        night,
        midnight,
        afterMidnight,
        dawn
    }

    public TimeOfDay timeOfDay { get; private set; }

    public float currentTime { get; set; }

    public void Update()
    {
        currentTime = transform.localEulerAngles.x;

        CurrentTimeOfDay();
    }

    public void CurrentTimeOfDay()
    {
        if (currentTime < 2)
            timeOfDay = TimeOfDay.dawn;

        else if (currentTime > 2 && currentTime < 60)
            timeOfDay = TimeOfDay.morning;

        else if (currentTime > 60 && currentTime < 120)
            timeOfDay = TimeOfDay.afternoon;

        else if (currentTime > 120 && currentTime < 178)
            timeOfDay = TimeOfDay.evening;

        else if (currentTime > 178 && currentTime < 180)
            timeOfDay = TimeOfDay.dusk;

        else if (currentTime > 180 && currentTime < 240)
            timeOfDay = TimeOfDay.night;

        else if (currentTime > 240 && currentTime < 300)
            timeOfDay = TimeOfDay.midnight;

        else if (currentTime > 300 && currentTime < 360)
            timeOfDay = TimeOfDay.afterMidnight;

        else
            currentTime = 0;
    }
}
