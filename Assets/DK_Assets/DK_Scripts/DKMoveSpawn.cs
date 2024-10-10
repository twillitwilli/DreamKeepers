using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DKMoveSpawn : MonoBehaviour
{
    [SerializeField]
    int newSceneSpawnIndex;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;

        // Check to see if player enters trigger
        if (other.gameObject.TryGetComponent<PlayerController>(out player))
        {
            // sets spawn location for new scene
            DKGameManager.Instance.spawnLocation = newSceneSpawnIndex;
        }
    }
}
