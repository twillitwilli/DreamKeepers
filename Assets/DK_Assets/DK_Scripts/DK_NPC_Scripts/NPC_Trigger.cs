using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPC_Trigger : MonoBehaviour
{
    
    protected NPCController _currentNPC;

    virtual public async void OnTriggerEnter(Collider other)
    {
        NPCController npc;
        if (!_currentNPC && other.gameObject.TryGetComponent<NPCController>(out npc))
        {
            _currentNPC = npc;
            NPCTriggerEntered(npc);
        }
    }

    virtual public async void OnTriggerExit(Collider other)
    {
        NPCController npc;
        if (_currentNPC != null && _currentNPC == other.gameObject.TryGetComponent<NPCController>(out npc))
        {
            NPCTriggerLeft(npc);
            _currentNPC = null;
        }
    }

    virtual public async void NPCTriggerEntered(NPCController npc)
    {

    }

    virtual public async void NPCTriggerLeft(NPCController npc)
    {

    }
}
