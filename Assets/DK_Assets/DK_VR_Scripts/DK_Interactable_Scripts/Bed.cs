using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Bed : MonoBehaviour
{
    [SerializeField]
    bool _sleepableBed;

    public DKSceneLoader.SceneSelection whichScene;

    [SerializeField]
    int newSceneSpawnIndex;

    public void Sleep()
    {
        if (_sleepableBed)
        {
            // sets spawn location for new scene
            DKGameManager.Instance.spawnLocation = newSceneSpawnIndex;

            // sends command to change scene
            DKSceneLoader.Instance.ChangeScene(whichScene);
        }

        else
            Debug.Log("This is not your bed to sleep in!");
    }
}
