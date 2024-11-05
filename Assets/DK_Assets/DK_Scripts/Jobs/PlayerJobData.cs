using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class PlayerJobData : MonoSingleton<PlayerJobData>
{
    public JobData[] jobs;

    public List<QuestData> currentJobs;

    public bool GetNewJob(QuestData questData)
    {
        QuestData newQuest = new QuestData();

        newQuest.jobType = questData.jobType;
        newQuest.requiredQuestItems = questData.requiredQuestItems;
        newQuest.goldReward = questData.goldReward;
        newQuest.expReward = questData.expReward;

        if (currentJobs.Count < 2)
        {
            if (currentJobs.Count == 0)
            {
                currentJobs.Add(newQuest);
                return true;
            }

            else
            {
                for (int i = 0; i < currentJobs.Count; i++)
                {
                    if (currentJobs[i].jobType == questData.jobType)
                        return false;

                    else
                    {
                        currentJobs.Add(newQuest);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void JobCompleted(JobData.JobType job)
    {
        for (int i = 0; i < currentJobs.Count; i++)
        {
            // find current job that is completed
            if (currentJobs[i].jobType == job)
            {
                // Give player gold reward
                PlayerController.Instance.playerStats.AdjustGold(currentJobs[i].goldReward);

                LevelUp(job, currentJobs[i].expReward);

                currentJobs.Remove(currentJobs[i]);

                return;
            }
        }
    }

    public void LevelUp(JobData.JobType job, float exp)
    {
        // casts enum to int
        int whichJob = (int)job;

        // adds exp gained to job completed
        jobs[whichJob].jobExp += exp;

        // checks to see if you got enough exp to level up your job
        if (LevelUpCheck(jobs[whichJob].jobExp, jobs[whichJob].jobLevel))
        {
            jobs[whichJob].jobLevel++;
            jobs[whichJob].jobExp = 0;
        }
    }

    bool LevelUpCheck(float currentExp, int currentLevel)
    {
        bool levelUp = currentExp >= (100 * currentLevel) ? true : false;
        return levelUp;
    }
}
