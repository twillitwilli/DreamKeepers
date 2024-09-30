using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.StopPlayback();
    }

    public void OpenChest()
    {
        _animator.Play("ChestOpening");
    }

    public void ChestOpened()
    {
        Debug.Log("Chest Opened, Get a reward");
    }
}
