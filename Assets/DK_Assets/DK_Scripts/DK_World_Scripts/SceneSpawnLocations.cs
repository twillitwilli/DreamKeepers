using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SoT.AbstractClasses;

public class SceneSpawnLocations : MonoSingleton<SceneSpawnLocations>
{
    [SerializeField]
    GameObject
        _gameManagerPrefab,
        _playerPrefab;

    private void Awake()
    {
        if (DKGameManager.Instance == null)
        {
            GameObject newManager = Instantiate(_gameManagerPrefab);
        }

        if (PlayerController.Instance == null)
        {
            GameObject newPlayer = Instantiate(_playerPrefab);
        }
    }

    public SpawnPosition[] spawnLocations;

    private async void Start()
    {
        // wait half a second before proceeding
        await Task.Delay(1000);

        // Gets GameManager
        DKGameManager gameManager = DKGameManager.Instance;

        // Gets player
        PlayerController player = PlayerController.Instance;

        // get spawn location from game manager
        int spawnLocation = gameManager.spawnLocation;

        // set current time
        DKTime.Instance.transform.localEulerAngles = new Vector3(gameManager.currentGameTime, 0, 0);

        // move player and adjust player rotation to spawn location
        player.transform.position = spawnLocations[spawnLocation].spawnLocation.position;
        player.transform.rotation = spawnLocations[spawnLocation].spawnLocation.rotation;

        // Gets player screen effects component
        PlayerScreenEffects screenEffects = player.head.GetComponent<PlayerScreenEffects>();

        // Clears players vision that is applied during changing scenes
        screenEffects.ClearVision();

        // Displays name of new area entered
        screenEffects.DisplayTextOnScreen(gameManager.nameOfCurrentArea);
    }
}
