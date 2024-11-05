using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScrollTrigger : MonoBehaviour
{
    public NPCController shopOwner { get; set; }

    public JobData.JobType shopType;

    [SerializeField]
    GameObject _jobToolPrefab;

    private void OnTriggerEnter(Collider other)
    {
        QuestScroll questScroll;
        EquipableItem questTool;

        // checks to see if there is a shop owner
        if (shopOwner != null)
        {
            // checks to see if the item is a quest scroll
            if (other.gameObject.TryGetComponent<QuestScroll>(out questScroll))
            {
                // checks to see if the player can get this new job and gives the job to the player while checking
                if (PlayerJobData.Instance.GetNewJob(questScroll.currentQuest))
                {
                    shopOwner.talkToNpc.currentTypeOfDialogue = TalkToNPC.DialogueType.jobAccepted;
                    shopOwner.talkToNpc.Talk();

                    AssignTool(questScroll);

                    Destroy(questScroll.gameObject);
                }

                // if player cant get that job
                else
                {
                    shopOwner.talkToNpc.currentTypeOfDialogue = TalkToNPC.DialogueType.cantGetJob;
                    shopOwner.talkToNpc.Talk();
                }

                shopOwner.talkToNpc.currentTypeOfDialogue = TalkToNPC.DialogueType.quest;
            }

            else if (other.gameObject.TryGetComponent<EquipableItem>(out questTool))
            {
                switch (shopType)
                {
                    case JobData.JobType.gatherer:

                        Bucket bucket;

                        // checks to see if quest tool is a bucket and if so checks to see if its full to complete the job
                        if (questTool.gameObject.TryGetComponent<Bucket>(out bucket) && bucket.bucketFull)
                        {
                            shopOwner.talkToNpc.currentTypeOfDialogue = TalkToNPC.DialogueType.jobCompleted;
                            shopOwner.talkToNpc.Talk();

                            //Complete quest
                            PlayerJobData.Instance.JobCompleted(JobData.JobType.gatherer);

                            Destroy(bucket.gameObject);
                        }

                        break;
                }
            }
        }
    }

    public void AssignShopOwner(NPCController npc)
    {
        shopOwner = npc;
        shopOwner.talkToNpc.currentTypeOfDialogue = TalkToNPC.DialogueType.quest;
    }

    public void ShopOwnerLeft()
    {
        shopOwner.talkToNpc.currentTypeOfDialogue = TalkToNPC.DialogueType.normalChat;
        shopOwner = null;
    }

    void AssignTool(QuestScroll questScroll)
    {
        GameObject newTool = Instantiate(_jobToolPrefab, transform.position, transform.rotation);

        switch (shopType)
        {
            case JobData.JobType.gatherer:

                Bucket bucket = newTool.GetComponent<Bucket>();

                bucket.lookingForItem = questScroll.currentQuest.questDescription;
                bucket.valueNeededForQuest = questScroll.currentQuest.requiredQuestItems;

                break;
        }
    }
}
