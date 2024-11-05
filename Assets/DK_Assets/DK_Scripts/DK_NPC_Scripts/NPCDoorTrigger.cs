using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPCDoorTrigger : NPC_Trigger
{
    [SerializeField]
    NPCDoorTrigger _oppositeSideOfDoor;

    public bool canEnterDoor { get; set; } = true;

    public override async void NPCTriggerEntered(NPCController npc)
    {
        _oppositeSideOfDoor.canEnterDoor = false;

        if (canEnterDoor)
        {
            npc.transform.position = _oppositeSideOfDoor.transform.position;
            npc.FindNewPathFromDoor();

            // wait 5 seconds then enable can enter door
            await Task.Delay(5000);
            _oppositeSideOfDoor.canEnterDoor = true;
        }
    }
}
