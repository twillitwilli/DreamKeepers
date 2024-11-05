using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
public class TalkToNPC : MonoBehaviour
{
    NPCController _npc;

    public enum DialogueType
    {
        normalChat,
        cantGetJob,
        jobAccepted,
        jobCompleted,
        quitJob,
        quest
    }

    public DialogueType currentTypeOfDialogue;

    [SerializeField]
    DialogueOptions _dialogueOptions;

    [SerializeField]
    GameObject _chatBubble;

    [SerializeField]
    Text _dialogueChat;

    public bool randomizeChat { get; set; } = true;
    public int questDialogueIdx { get; set; }

    private void Awake()
    {
        _npc = GetComponent<NPCController>();
    }

    public async void Talk()
    {
        if (_npc.currentDestination.canIteractWithPlayer)
        {
            Debug.Log("Talking to player");

            _chatBubble.SetActive(true);

            switch (currentTypeOfDialogue)
            {
                case DialogueType.normalChat:

                    if (randomizeChat)
                        _dialogueChat.text = _dialogueOptions.talking[0].dialogue[GetRandomDialogue(_dialogueOptions.talking[0].dialogue.Length)];

                    else
                        _dialogueChat.text = _dialogueOptions.talking[0].dialogue[questDialogueIdx];

                    break;

                case DialogueType.cantGetJob:

                    if (randomizeChat)
                        _dialogueChat.text = _dialogueOptions.talking[1].dialogue[GetRandomDialogue(_dialogueOptions.talking[1].dialogue.Length)];

                    else
                        _dialogueChat.text = _dialogueOptions.talking[1].dialogue[questDialogueIdx];

                    break;

                case DialogueType.jobAccepted:

                    if (randomizeChat)
                        _dialogueChat.text = _dialogueOptions.talking[2].dialogue[GetRandomDialogue(_dialogueOptions.talking[2].dialogue.Length)];

                    else
                        _dialogueChat.text = _dialogueOptions.talking[2].dialogue[questDialogueIdx];

                    break;

                case DialogueType.jobCompleted:

                    if (randomizeChat)
                        _dialogueChat.text = _dialogueOptions.talking[3].dialogue[GetRandomDialogue(_dialogueOptions.talking[3].dialogue.Length)];

                    else
                        _dialogueChat.text = _dialogueOptions.talking[3].dialogue[questDialogueIdx];

                    break;

                case DialogueType.quitJob:

                    if (randomizeChat)
                        _dialogueChat.text = _dialogueOptions.talking[4].dialogue[GetRandomDialogue(_dialogueOptions.talking[4].dialogue.Length)];

                    else
                        _dialogueChat.text = _dialogueOptions.talking[4].dialogue[questDialogueIdx];

                    break;

                case DialogueType.quest:

                    if (randomizeChat)
                        _dialogueChat.text = _dialogueOptions.talking[5].dialogue[GetRandomDialogue(_dialogueOptions.talking[5].dialogue.Length)];

                    else
                        _dialogueChat.text = _dialogueOptions.talking[5].dialogue[questDialogueIdx];

                    break;
            }

            await Task.Delay(8000);

            _chatBubble.SetActive(false);
        }

        else
            Debug.Log("They dont want to talk to you");
    }

    int GetRandomDialogue(int totalOptions)
    {
        int randomDialogue = Random.Range(0, totalOptions);

        return randomDialogue;
    }
}
