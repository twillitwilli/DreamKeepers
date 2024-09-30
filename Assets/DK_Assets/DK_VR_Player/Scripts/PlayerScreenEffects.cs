using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenEffects : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    Animator _blindnessAnimator;

    bool _headInsideObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!_headInsideObject && other.gameObject.CompareTag("Wall"))
        {
            _headInsideObject = true;
            _playerController.disableMovement = true;
            _blindnessAnimator.gameObject.SetActive(true);
            _blindnessAnimator.Play("FadeVisionOut");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_headInsideObject && other.gameObject.CompareTag("Wall"))
        {
            _headInsideObject = false;
            _playerController.disableMovement = false;
            _blindnessAnimator.Play("FadeVisionIn");
        }
    }

    public void BlindnessVisionClear()
    {
        _blindnessAnimator.gameObject.SetActive(false);
    }
}
