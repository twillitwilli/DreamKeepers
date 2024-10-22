using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
public class TalkToNPC : MonoBehaviour
{
    NPCController _npc;

    [SerializeField]
    string[]
        _npcQuestDialogue,
        _randomizedDialogue;

    [SerializeField]
    bool _randomizeDialogue;

    private void Awake()
    {
        _npc = GetComponent<NPCController>();
    }

    public void Talk()
    {
        if (_npc.currentDestination.canIteractWithPlayer)
        {
            Debug.Log("Talking to player");

            // Checks to see if the dialogue will be randomized
            if (_randomizeDialogue)
                RandomizeDialogue();

            else
                QuestDialogue();
        }

        else
            Debug.Log("They dont want to talk to you");
    }

    void RandomizeDialogue()
    {
        Debug.Log("Randomized Dialogue");
    }

    void QuestDialogue()
    {
        Debug.Log("Quest Dialogue");
    }
}
