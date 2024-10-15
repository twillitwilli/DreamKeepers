using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLoadGameTrigger : MonoBehaviour
{
    [SerializeField]
    int _saveFile;

    public void OnTriggerEnter(Collider other)
    {
        PlayerController player;

        if (other.gameObject.TryGetComponent<PlayerController>(out player))
        {
            // sets current save file ID
            DKGameManager.Instance.saveFile = _saveFile;

            // Check to see if this is a new game or loading previous game
            BinarySaveData loadedData = BinarySaveSystem.LoadData(_saveFile);

            // Found Game Save
            if (loadedData != null) 
                BinaryPlayerSaveLoad.Instance.LoadSavedData(loadedData);

            // Starts New Game
            else
                DKSceneLoader.Instance.ChangeScene(DKSceneLoader.SceneSelection.NightmareNamikVillage, true);
        }
    }
}
