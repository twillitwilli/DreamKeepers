using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DKSceneTrigger : MonoBehaviour
{
    public DKSceneLoader.SceneSelection whichScene;

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

            // sends command to change scene
            DKSceneLoader.Instance.ChangeScene(whichScene);
        }
    }
}
