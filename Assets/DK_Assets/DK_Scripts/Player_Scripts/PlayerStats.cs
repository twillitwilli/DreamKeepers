using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    // Health
    public float health { get; private set; }
    public float healthCrystals { get; private set; }
    public float maxHealth { get; private set; }
    public float armor { get; private set; }

    // Gold
    public int gold { get; private set; }
    public int maxGold { get; private set; }

    // Arrows
    public int arrows { get; private set; }
    public int maxArrows { get; private set; }


    private void Start()
    {
        DefaultValues();
    }

    void DefaultValues()
    {
        // Health Values
        maxHealth = 300;
        health = maxHealth;
        healthCrystals = 0;
        armor = 0;

        // Gold Values
        gold = 0;
        maxGold = 5000;

        // Arrow Values
        arrows = 0;
        maxArrows = 0;
    }

    // ------------------ Health Functions --------------------------

    public void AdjustCurrentHealth(bool damaged, float healthVal)
    {
        // adjusts damage amount based on current armor
        if (damaged && armor > 0)
            healthVal -= healthVal * armor;

        health += healthVal;

        // player damaged
        if (damaged)
        {
            Debug.Log("Player Took Damage");

            if (health <= 0)
                Death();
        }

        // player obtained health
        else
        {
            if (health > maxHealth)
                health = maxHealth;
        }
    }

    void Death()
    {
        Debug.Log("Player Died");
    }

    public void HealthCrystalObtained()
    {
        healthCrystals++;

        // 5 health crystals will increase max health by 100
        if (healthCrystals == 5)
        {
            healthCrystals = 0;
            maxHealth += 100;
            health = maxHealth;
        }
    }

    public void AdjustArmor(float armorVal)
    {
        armor += armorVal;

        // Max armor 50% less damage
        if (armor > 0.5f)
            armor = 0.5f;
    }

    // ----------------------- Gold Functions ------------------------

    public void AdjustGold(int goldVal)
    {
        gold += goldVal;
        if (gold > maxGold)
            gold = maxGold;
    }

    public void ObtainedNewWallet()
    {
        maxGold *= 2;
    }

    // ---------------------- Arrow Functions --------------------------

    public void AdjustArrows(int arrowVal)
    {
        arrows += arrowVal;

        if (arrows > maxArrows)
            arrows = maxArrows;
    }

    public void ArrowUpgrade()
    {
        maxArrows += 16;
    }
}
