using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPCDoorTrigger : MonoBehaviour
{
    [SerializeField]
    NPCDoorTrigger _oppositeSideOfDoor;

    public bool canEnterDoor { get; set; } = true;

    private async void OnTriggerEnter(Collider other)
    {
        _oppositeSideOfDoor.canEnterDoor = false;

        NPCController npc;
        // checks for npc entering door
        if (canEnterDoor && other.gameObject.TryGetComponent<NPCController>(out npc))
        {
            npc.transform.position = _oppositeSideOfDoor.transform.position;
            npc.FindNewPathFromDoor();

            // wait 5 seconds then enable can enter door
            await Task.Delay(5000);
            _oppositeSideOfDoor.canEnterDoor = true;
        }
    }
}
