using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JobData
{
    public enum JobType
    {
        gatherer,
        lumberjack,
        hunter,
        monsterHunter
    }

    public JobType typeOfJob;

    public int jobLevel;

    public float jobExp;
}
