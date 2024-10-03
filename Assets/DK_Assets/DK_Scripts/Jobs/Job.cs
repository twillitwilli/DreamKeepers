using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour
{
    public int
        goldReward,
        experienceReward;

    public virtual void JobAccepted()
    {
        Debug.Log("Job Accepted");
    }

    public virtual void JobComplete()
    {
        Debug.Log("Job Completed");
    }
}
