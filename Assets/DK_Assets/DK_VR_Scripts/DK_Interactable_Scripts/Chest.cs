using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Chest : MonoBehaviour
{
    Animator _animator;

    bool _chestOpened;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (!_chestOpened)
        {
            _animator.Play("ChestOpening");
            _chestOpened = true;
        }
    }
}
