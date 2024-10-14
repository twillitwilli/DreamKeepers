using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySaveStats : MonoBehaviour
{
    [SerializeField]
    int _saveFile;

    [SerializeField]
    Text _text;

    private void Start()
    {
        if (DKSaveLoad.Instance.CheckForFileSave(_saveFile) != null)
        {
            DKBinarySaveData loadedData = DKBinarySaveSystem.LoadData(_saveFile);

            _text.text = "Save File #" + _saveFile + "\n" + loadedData.playerName;
        }
    }
}
