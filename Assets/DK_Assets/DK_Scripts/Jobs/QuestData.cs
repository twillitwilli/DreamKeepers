using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public JobData.JobType jobType;

    public string questDescription;

    public int
        requiredQuestItems,
        goldReward,
        expReward;
}
