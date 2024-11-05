using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScroll : MonoBehaviour
{
    [SerializeField]
    Text
        _jobTitle,
        _jobDescription,
        _jobReward;

    public JobData.JobType currentJob;

    public string jobName { get; private set; }

    public QuestData[] questData;

    public QuestData currentQuest { get; private set; }

    private void Start()
    {
        UpdateQuestScroll();

        Debug.Log(currentJob);
    }

    public void UpdateQuestScroll()
    {
        int randomizeQuest = Random.Range(0, questData.Length);

        currentQuest = questData[randomizeQuest];

        // name of job
        _jobTitle.text = "-- " + jobName + " --";

        // job description
        _jobDescription.text =
            "- Quest -" +
            "\n" +
            currentQuest.requiredQuestItems +
            "\n" +
            currentQuest.questDescription;

        // job rewards
        _jobReward.text =
            "- Reward -" +
            "\n" +
            currentQuest.goldReward + " Gold" +
            "\n" +
            currentQuest.expReward + " Exp";
    }
}
