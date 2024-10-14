using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    public PlayerData stats;

    private void Start()
    {
        DefaultValues();
    }

    void DefaultValues()
    {
        // Health Values
        stats.maxHealth = 300;
        stats.health = stats.maxHealth;
        stats.healthCrystals = 0;
        stats.armor = 0;

        // Gold Values
        stats.gold = 0;
        stats.maxGold = 5000;

        // Arrow Values
        stats.arrows = 0;
        stats.maxArrows = 0;
    }

    // ------------------ Health Functions --------------------------

    public void RefillHealth()
    {
        stats.health = stats.maxHealth;
    }

    public void NightmareHealth()
    {
        stats.health = 1;
    }

    public void AdjustCurrentHealth(bool damaged, float healthVal)
    {
        // adjusts damage amount based on current armor
        if (damaged && stats.armor > 0)
            healthVal -= healthVal * stats.armor;

        stats.health += healthVal;

        // player damaged
        if (damaged)
        {
            Debug.Log("Player Took Damage");

            if (stats.health <= 0)
                Death();
        }

        // player obtained health
        else
        {
            if (stats.health > stats.maxHealth)
                stats.health = stats.maxHealth;
        }
    }

    void Death()
    {
        // if player dies in a dream, they wake up
        if (DKGameManager.Instance.isNightmare)
        {
            switch (DKSceneLoader.Instance.currentScene)
            {
                case DKSceneLoader.SceneSelection.NightmareNamikVillage:

                    DKSceneLoader.Instance.ChangeScene(DKSceneLoader.SceneSelection.NamikVillage);

                    break;
            }
        }

        else
        {
            Debug.Log("Player Died");
        }
    }

    public void HealthCrystalObtained()
    {
        stats.healthCrystals++;

        // 5 health crystals will increase max health by 100
        if (stats.healthCrystals == 5)
        {
            stats.healthCrystals = 0;
            stats.maxHealth += 100;
            stats.health = stats.maxHealth;
        }
    }

    public void AdjustArmor(float armorVal)
    {
        stats.armor += armorVal;

        // Max armor 50% less damage
        if (stats.armor > 0.5f)
            stats.armor = 0.5f;
    }

    // ----------------------- Gold Functions ------------------------

    public void AdjustGold(int goldVal)
    {
        stats.gold += goldVal;
        if (stats.gold > stats.maxGold)
            stats.gold = stats.maxGold;
    }

    public void ObtainedNewWallet()
    {
        stats.maxGold *= 2;
    }

    // ---------------------- Arrow Functions --------------------------

    public void AdjustArrows(int arrowVal)
    {
        stats.arrows += arrowVal;

        if (stats.arrows > stats.maxArrows)
            stats.arrows = stats.maxArrows;
    }

    public void ArrowUpgrade()
    {
        stats.maxArrows += 16;
    }
}
