using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveFile : MonoBehaviour
{
    [SerializeField]
    int _saveFile;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;

        if (other.gameObject.TryGetComponent<PlayerController>(out player))
        {
            BinarySaveSystem.DeleteFileSave(_saveFile);
        }
    }
}
