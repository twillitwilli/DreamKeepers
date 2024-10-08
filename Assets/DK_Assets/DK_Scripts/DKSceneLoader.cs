using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoT.AbstractClasses;

public class DKSceneLoader : MonoSingleton<DKSceneLoader>
{
    public enum SceneSelection
    {
        TitleScreen,
        NightmareNamikVillage,
        NamikVillage,
        TeleportNexus,
        NamikCanyon
    }

    public async void ChangeScene(SceneSelection whichScene)
    {
        // Closes Players Vision
        PlayerController.Instance.head.GetComponent<PlayerScreenEffects>().CloseVision();

        // wait 3 seconds before changing scene
        await Task.Delay(3000);

        // casts enum to an int to change to scene in the build list
        SceneManager.LoadScene((int)whichScene);
    }
}
