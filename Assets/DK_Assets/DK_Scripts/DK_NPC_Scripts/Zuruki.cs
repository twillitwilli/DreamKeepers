using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Zuruki : DialogueTrigger
{
    [SerializeField]
    GameObject _parentBody;

    [SerializeField]
    ParticleSystem _particleSystem;

    public override void StartDialogue()
    {
        base.StartDialogue();
    }

    public override void FinishedTalking()
    {
        base.FinishedTalking();

        Disappear();
    }

    async void Disappear()
    {

        // wait 5 seconds then destroy;
        await Task.Delay(5000);

        Destroy(_parentBody);
    }
}
