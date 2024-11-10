using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;
using SoT.Classes;

public class DKTime : MonoSingleton<DKTime>
{
    public delegate void TimeOfDayChanged();
    public static event TimeOfDayChanged timeChanged;

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

    public bool isNight { get; private set; }

    private void Start()
    {
        // sets the correct time of day when entering a new scene
        if (!DKGameManager.Instance.isNightmare)
            transform.localEulerAngles = new Vector3(DKGameManager.Instance.currentGameTime, 0, 0);

        // if nightmare sets static time
        else
            transform.localEulerAngles = new Vector3(45, 0, 0);
    }

    public void Update()
    {
        if (!DKGameManager.Instance.isNightmare)
        {
            currentTime = transform.localEulerAngles.x;

            CurrentTimeOfDay();

            if (currentTime > 180 && !isNight)
                ToggleDayNight(true);

            else if (currentTime > 0 && currentTime < 180 && isNight)
                ToggleDayNight(false);
        }
    }

    public void CurrentTimeOfDay()
    {
        if (currentTime > 0 && currentTime < 2 && timeOfDay != TimeOfDay.dawn)
        {
            timeOfDay = TimeOfDay.dawn;
            if (timeChanged != null)
                timeChanged();
        } 

        else if (currentTime > 2 && currentTime < 60 && timeOfDay != TimeOfDay.morning)
        {
            timeOfDay = TimeOfDay.morning;
            if (timeChanged != null)
                timeChanged();
        }
            

        else if (currentTime > 60 && currentTime < 120 && timeOfDay != TimeOfDay.afternoon)
        {
            timeOfDay = TimeOfDay.afternoon;
            if (timeChanged != null)
                timeChanged();
        }
            

        else if (currentTime > 120 && currentTime < 178 && timeOfDay != TimeOfDay.evening)
        {
            timeOfDay = TimeOfDay.evening;
            if (timeChanged != null)
                timeChanged();
        }
            

        else if (currentTime > 178 && currentTime < 180 && timeOfDay != TimeOfDay.dusk)
        {
            timeOfDay = TimeOfDay.dusk;
            if (timeChanged != null)
                timeChanged();
        }
            

        else if (currentTime > 180 && currentTime < 240 && timeOfDay != TimeOfDay.night)
        {
            timeOfDay = TimeOfDay.night;
            if (timeChanged != null)
                timeChanged();
        }
            
        else if (currentTime > 240 && currentTime < 300 && timeOfDay != TimeOfDay.midnight)
        {
            timeOfDay = TimeOfDay.midnight;
            if (timeChanged != null)
                timeChanged();
        }
            
        else if (currentTime > 300 && currentTime < 360 && timeOfDay != TimeOfDay.afterMidnight)
        {
            timeOfDay = TimeOfDay.afterMidnight;
            if (timeChanged != null)
                timeChanged();
        }

        else
            currentTime = 0;
    }

    void ToggleDayNight(bool night)
    {
        if (night)
        {
            // night time is shorter than day time
            rotationController.rotationSpeed = -0.35f;

            isNight = true;
        }

        else
        {
            rotationController.rotationSpeed = -0.25f;
            isNight = false;
        }
    }
}
