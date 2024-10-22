using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        bed,
        chest,
        door,
        npc
    }

    public InteractableType typeOfInteractable;

    public void Interact()
    {
        // Checks which type of interaction this is
        switch (typeOfInteractable)
        {
            // Bed
            case InteractableType.bed:

                BedInteraction();

                break;

            // Chest
            case InteractableType.chest:

                ChestInteraction();

                break;

            // Door
            case InteractableType.door:

                DoorInteraction();

                break;

            // NPC
            case InteractableType.npc:

                NPCInteraction();

                break;
        }
    }

    public void BedInteraction()
    {
        Bed bed = GetComponent<Bed>();
        bed.Sleep();
    }

    public void ChestInteraction()
    {
        // Get Chest
        Chest chest = GetComponent<Chest>();
        chest.OpenChest();
    }

    public void DoorInteraction()
    {
        VRDoor door = GetComponent<VRDoor>();
        door.UnlockDoor();
    }

    public void NPCInteraction()
    {
        TalkToNPC npc = GetComponent<TalkToNPC>();
        npc.Talk();
    }
}
