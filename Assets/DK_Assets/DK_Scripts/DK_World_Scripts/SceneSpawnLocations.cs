using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SoT.AbstractClasses;

public class SceneSpawnLocations : MonoSingleton<SceneSpawnLocations>
{
    public SpawnPosition[] spawnLocations;

    private async void Start()
    {
        // wait half a second before proceeding
        await Task.Delay(500);

        // get spawn location from game manager
        int spawnLocation = DKGameManager.Instance.spawnLocation;

        // move player and adjust player rotation to spawn location
        PlayerController.Instance.transform.position = spawnLocations[spawnLocation].spawnLocation.position;
        PlayerController.Instance.transform.rotation = spawnLocations[spawnLocation].spawnLocation.rotation;

        // Clears players vision that is applied during changing scenes
        PlayerController.Instance.head.GetComponent<PlayerScreenEffects>().ClearVision();
    }
}
