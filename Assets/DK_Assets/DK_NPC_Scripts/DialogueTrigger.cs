using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    PlayerController _player;

    [SerializeField]
    bool _canStopPlayer;

    [SerializeField]
    string[] _dialogue;

    int _currentDialogue;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out _player))
        {
            if (_canStopPlayer)
                _player.disableMovement = true;

            StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player.gameObject)
            _player = null;
    }

    public virtual void StartDialogue()
    {
        _currentDialogue = 0;

        Debug.Log(_dialogue[0]);
    }

    public void ContinueDialogue()
    {
        _currentDialogue++;

        // checks to make sure there is dialogue options
        if (_currentDialogue < _dialogue.Length)
            Debug.Log(_dialogue[_currentDialogue]);

        else
            FinishedTalking();
    }

    public virtual void FinishedTalking()
    {
        if (_canStopPlayer)
            _player.disableMovement = false;
    }
}
