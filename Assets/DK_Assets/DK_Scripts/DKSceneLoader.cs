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
        NamikCanyon,
        Luruna
    }

    SceneSelection _currentScene;

    string GetSceneName()
    {
        switch (_currentScene)
        {
            case SceneSelection.TitleScreen:
                return "Dream Keepers";

            case SceneSelection.NightmareNamikVillage:
                return "Namik Village";

            case SceneSelection.NamikVillage:
                return "Namik Village";

            case SceneSelection.TeleportNexus:
                return "Teleport Nexus";

            case SceneSelection.NamikCanyon:
                return "Namik Canyon";

            case SceneSelection.Luruna:
                return "Lunruna";
        }

        return "Error: No Name Found";
    }

    public async void ChangeScene(SceneSelection whichScene)
    {
        // changes new scene to current scene
        _currentScene = whichScene;

        // sets the name of the new area
        DKGameManager.Instance.nameOfCurrentArea = GetSceneName();

        // Closes Players Vision
        PlayerController.Instance.head.GetComponent<PlayerScreenEffects>().CloseVision();

        // wait 3 seconds before changing scene
        await Task.Delay(3000);

        // casts enum to an int to change to scene in the build list
        SceneManager.LoadScene((int)whichScene);
    }


}
