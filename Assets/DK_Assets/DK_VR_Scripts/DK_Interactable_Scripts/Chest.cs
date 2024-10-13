using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator _animator;

    bool _chestOpened;

    [SerializeField]
    GameObject _itemRewardPrefab;

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

    public void ChestOpened()
    {
        Debug.Log("Chest Opened, Get a reward");
        Instantiate(_itemRewardPrefab, transform.position, transform.rotation);
    }
}
