using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPCBedTrigger : MonoBehaviour
{
    NPCController _currentNPC;

    [SerializeField]
    Transform sleepPosition;

    private async void OnTriggerEnter(Collider other)
    {
        NPCController npc;
        if (!_currentNPC && other.gameObject.TryGetComponent<NPCController>(out npc))
        {
            //Getting into bed
            _currentNPC = npc;
            _currentNPC.GetComponent<CapsuleCollider>().isTrigger = true;

            // wait 3 seconds
            await Task.Delay(3000);

            // npc sleeping
            _currentNPC.nPCModel.transform.position = sleepPosition.position;
            _currentNPC.nPCModel.transform.rotation = sleepPosition.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NPCController npc;
        if (_currentNPC != null && _currentNPC == other.gameObject.TryGetComponent<NPCController>(out npc))
        {
            _currentNPC.nPCModel.transform.localPosition = new Vector3(0, 0, 0);
            _currentNPC.nPCModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            _currentNPC.GetComponent<CapsuleCollider>().isTrigger = false;
        }
    }
}
