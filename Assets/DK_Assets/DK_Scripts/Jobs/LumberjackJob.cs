using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberjackJob : Job
{
    public bool
        lumberjackJobAccepted,
        lumberjackJobCompleted;

    private void Update()
    {
        if (lumberjackJobAccepted)
        {
            JobAccepted();
            lumberjackJobAccepted = false;
        }

        if (lumberjackJobCompleted)
        {
            JobComplete();
            lumberjackJobCompleted = false;
        }
    }

    public override void JobAccepted()
    {
        base.JobAccepted();

        Debug.Log("Lumberjack Job Accepted");
    }

    public override void JobComplete()
    {
        base.JobComplete();

        PlayerJobData.Instance.LevelUp(PlayerJobData.PlayerJobs.Lumberjack, experienceReward);
        PlayerStats.Instance.AdjustGold(goldReward);
    }
}
