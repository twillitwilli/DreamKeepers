using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class PlayerJobData : MonoSingleton<PlayerJobData>
{
    public enum PlayerJobs
    {
        Lumberjack,
        Gatherer,
        Hunter,
        MonsterHunter
    }

    public JobData[] jobs;

    public void LevelUp(PlayerJobs job, float exp)
    {
        // casts enum to int
        int whichJob = (int)job;

        // adds exp gained to job completed
        jobs[whichJob].jobExp += exp;

        // checks to see if you got enough exp to level up your job
        if (LevelUpCheck(jobs[whichJob].jobExp))
        {
            Debug.Log(jobs[whichJob].jobName + " Level Up");

            jobs[whichJob].jobLevel++;
            jobs[whichJob].jobExp = 0;
        }
    }

    bool LevelUpCheck(float currentExp)
    {
        bool levelUp = currentExp >= 100 ? true : false;
        return levelUp;
    }
}
