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
        Luruna,
        DalamikGame
    }

    public SceneSelection currentScene { get; private set; }

    string GetSceneName()
    {
        switch (currentScene)
        {
            case SceneSelection.TitleScreen:

                TitleScreenProperties();

                return "Dream Keepers";

            case SceneSelection.NightmareNamikVillage:

                NightmareProperties();

                return "Nightmare";

            case SceneSelection.NamikVillage:

                WakingUpFromNightmareProperties();
                
                return "Namik Village";

            case SceneSelection.TeleportNexus:

                return "Teleport Nexus";

            case SceneSelection.NamikCanyon:

                WakingUpFromNightmareProperties();

                return "Namik Canyon";

            case SceneSelection.Luruna:

                WakingUpFromNightmareProperties();

                return "Lunruna";

            case SceneSelection.DalamikGame:

                return "Dalamik Game";
        }

        return "Error: No Name Found";
    }

    public async void ChangeScene(SceneSelection whichScene, bool newGameLoad = false)
    {
        // changes new scene to current scene
        currentScene = whichScene;

        // gets current game time
        float getTime = DKGameManager.Instance.isNightmare ? 0 : DKTime.Instance.currentTime;
        DKGameManager.Instance.currentGameTime = getTime;

        // sets the name of the new area
        DKGameManager.Instance.nameOfCurrentArea = GetSceneName();

        // sets current scene int
        DKGameManager.Instance.currentScene = (int)currentScene;

        // Closes Players Vision
        PlayerController.Instance.head.GetComponent<PlayerScreenEffects>().CloseVision();

        // Saves Game Data
        if (!newGameLoad)
            BinaryPlayerSaveLoad.Instance.SaveData(DKGameManager.Instance.saveFile);

        // wait 3 seconds before changing scene
        await Task.Delay(3000);

        // casts enum to an int to change to scene in the build list
        SceneManager.LoadScene((int)whichScene);
    }

    void TitleScreenProperties()
    {
        DKGameManager.Instance.isNightmare = false;
        PlayerController.Instance.playerStats.RefillHealth();

        DKGameManager.Instance._postProcessingProfiles.ChangePostProcessingProfile(PostProcessingProfiles.PostProcessingProfileType.BasicSetup);
    }

    void NightmareProperties()
    {
        DKGameManager.Instance.isNightmare = true;
        PlayerController.Instance.playerStats.NightmareHealth();

        DKGameManager.Instance._postProcessingProfiles.ChangePostProcessingProfile(PostProcessingProfiles.PostProcessingProfileType.Nightmare);
    }

    void WakingUpFromNightmareProperties()
    {
        // if player is waking up from a nightmare
        if (DKGameManager.Instance.isNightmare)
        {
            PlayerController.Instance.playerStats.RefillHealth();
            DKGameManager.Instance.isNightmare = false;

            DKGameManager.Instance._postProcessingProfiles.ChangePostProcessingProfile(PostProcessingProfiles.PostProcessingProfileType.NightTime);
        }
    }
}
