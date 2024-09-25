using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimatorController : MonoBehaviour
{
    Animator _handAnimator;

    private void Awake()
    {
        _handAnimator = GetComponent<Animator>();
    }

    public void ChangeAnimation(string animationName)
    {
        _handAnimator.Play(animationName);
    }
}
