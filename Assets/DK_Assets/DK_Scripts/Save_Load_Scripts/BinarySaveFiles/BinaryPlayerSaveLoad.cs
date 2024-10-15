using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class BinaryPlayerSaveLoad : MonoSingleton<BinaryPlayerSaveLoad>
{
    [SerializeField] 
    private bool 
        _saveData, 
        _loadData, 
        _deleteData;

    [SerializeField]
    string _playerName; 

    private void Update()
    {
        if (_saveData)
        {
            BinarySaveSystem.SaveData(CreateSaveData(), 0);

            _saveData = false;
        }

        if (_loadData)
        {
            BinarySaveData loadedData = BinarySaveSystem.LoadData(0);

            if (loadedData != null) { LoadSavedData(loadedData); }
            else { Debug.Log("No Save File Found"); }

            _loadData = false;
        }

        if (_deleteData)
        {
            BinarySaveSystem.DeleteFileSave(0);

            _deleteData = false;
        }
    }

    public void SaveData(int saveFile)
    {
        BinarySaveSystem.SaveData(CreateSaveData(), saveFile);
    }

    private BinarySaveData CreateSaveData()
    {
        // Gets Game Managers
        PlayerStats stats = PlayerController.Instance.playerStats;
        DKGameManager gameManager = DKGameManager.Instance;
        DKProgressionChecks progressionChecks = DKProgressionChecks.Instance;
        GameItems gameItems = gameManager.gameItems;
        PlayerJobData jobData = PlayerJobData.Instance;

        // Create new data file
        BinarySaveData newData = new BinarySaveData();

        newData.playerName = stats.stats.name;

        // World Save Data

        newData.saveFile = gameManager.saveFile;
        newData.currentScene = gameManager.currentScene;
        newData.currentGameTime = gameManager.currentGameTime;

        // Player Save Data
        newData.health = stats.stats.health;
        newData.healthCrystals = stats.stats.healthCrystals;
        newData.maxHealth = stats.stats.maxHealth;
        newData.armor = stats.stats.armor;

        newData.gold = stats.stats.gold;
        newData.maxGold = stats.stats.maxGold;

        newData.arrows = stats.stats.arrows;
        newData.maxArrows = stats.stats.maxArrows;

        // Unlocks

        // Main Quest Progression

        newData.mainQuestProgression = new bool[progressionChecks.mainQuestProgression.Length];

        for (int i = 0; i < progressionChecks.mainQuestProgression.Length; i++)
        {
            newData.mainQuestProgression[i] = progressionChecks.mainQuestProgression[i].completed;
        }

        // Nexus Portal Unlocks

        newData.nexusPortalUnlocks = new bool[progressionChecks.nexusPortalUnlocks.Length];

        for (int i = 0; i < progressionChecks.nexusPortalUnlocks.Length; i++)
        {
            newData.nexusPortalUnlocks[i] = progressionChecks.nexusPortalUnlocks[i].completed;
        }

        // Sword Unlocks

        newData.unlockedSwords = new bool[gameItems.swords.Count];

        for (int i = 0; i < gameItems.swords.Count; i++)
        {
            newData.unlockedSwords[i] = gameItems.swords[i].itemUnlocked;
        }

        // Bow Unlocks

        newData.unlockedBows = new bool[gameItems.bows.Count];

        for (int i = 0; i < gameItems.bows.Count; i++)
        {
            newData.unlockedBows[i] = gameItems.bows[i].itemUnlocked;
        }

        // Key Unlocks

        newData.unlockedKeys = new bool[gameItems.keys.Count];

        for (int i = 0; i < gameItems.keys.Count; i++)
        {
            newData.unlockedKeys[i] = gameItems.keys[i].itemUnlocked;
        }

        // Job Save Data

        newData.currentLevel = new int[jobData.jobs.Length];
        newData.currentExp = new float[jobData.jobs.Length];

        for (int i = 0; i < jobData.jobs.Length; i++)
        {
            newData.currentLevel[i] = jobData.jobs[i].jobLevel;
            newData.currentExp[i] = jobData.jobs[i].jobExp;
        }

        return newData;
    }

    public void LoadSavedData(BinarySaveData loadedData)
    {
        // Gets Game Managers
        PlayerStats stats = PlayerController.Instance.playerStats;
        DKGameManager gameManager = DKGameManager.Instance;
        DKProgressionChecks progressionChecks = DKProgressionChecks.Instance;
        GameItems gameItems = gameManager.gameItems;
        PlayerJobData jobData = PlayerJobData.Instance;

        stats.stats.name = loadedData.playerName;
        _playerName = loadedData.playerName;

        // World Save Data

        gameManager.currentScene = loadedData.currentScene;
        gameManager.currentGameTime = loadedData.currentGameTime;

        // Player Save Data

        stats.stats.health = loadedData.health;
        stats.stats.healthCrystals = loadedData.healthCrystals;
        stats.stats.maxHealth = loadedData.maxHealth;
        stats.stats.armor = loadedData.armor;

        stats.stats.gold = loadedData.gold;
        stats.stats.maxGold = loadedData.maxGold;

        stats.stats.arrows = loadedData.arrows;
        stats.stats.maxArrows = loadedData.maxArrows;

        // Unlocks

        // Main Quest Progression

        for (int i = 0; i < loadedData.mainQuestProgression.Length; i++)
        {
            progressionChecks.mainQuestProgression[i].completed = loadedData.mainQuestProgression[i];
        }

        // Nexus Portal Unlocks

        for (int i = 0; i < loadedData.nexusPortalUnlocks.Length; i++)
        {
            progressionChecks.nexusPortalUnlocks[i].completed = loadedData.nexusPortalUnlocks[i];
        }

        // Sword Unlocks

        for (int i = 0; i < loadedData.unlockedSwords.Length; i++)
        {
            gameItems.swords[i].itemUnlocked = loadedData.unlockedSwords[i];
        }

        // Bow Unlocks

        for (int i = 0; i < loadedData.unlockedBows.Length; i++)
        {
            gameItems.bows[i].itemUnlocked = loadedData.unlockedBows[i];
        }

        // Key Unlocks

        for (int i = 0; i < loadedData.unlockedKeys.Length; i++)
        {
            gameItems.keys[i].itemUnlocked = loadedData.unlockedKeys[i];
        }

        // Job Save Data

        for (int i = 0; i < loadedData.currentLevel.Length; i++)
        {
            jobData.jobs[i].jobLevel = loadedData.currentLevel[i];
            jobData.jobs[i].jobExp = loadedData.currentExp[i];
        }

        // Get Current Scene To Load

        DKSceneLoader.SceneSelection loadedScene = DKSceneLoader.SceneSelection.TitleScreen;

        switch (loadedData.currentScene)
        {
            case 1:
                loadedScene = DKSceneLoader.SceneSelection.NightmareNamikVillage;
                break;

            case 2:
                loadedScene = DKSceneLoader.SceneSelection.NamikVillage;
                break;

            case 3:
                loadedScene = DKSceneLoader.SceneSelection.TeleportNexus;
                break;

            case 4:
                loadedScene = DKSceneLoader.SceneSelection.NamikCanyon;
                break;

            case 5:
                loadedScene = DKSceneLoader.SceneSelection.Luruna;
                break;
        }

        if (loadedScene == DKSceneLoader.SceneSelection.TitleScreen)
            Debug.Log("load data for scene missing");

        DKSceneLoader.Instance.ChangeScene(loadedScene);
    }
}
