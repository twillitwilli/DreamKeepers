using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class PlayerJobData : MonoSingleton<PlayerJobData>
{
    public enum PlayerJobs
    {
        Lumberjack,
        Gatherer,
        Hunter,
        MonsterHunter
    }

    // Lumberjack
    public float lumberjackExp { get; private set; }
    public int lumberjackLevel { get; private set; }

    // Gatherer
    public float gathererExp { get; private set; }
    public int gathererLevel { get; private set; }

    // Hunter
    public float hunterExp { get; private set; }
    public int hunterLevel { get; private set; }

    // Monster Hunter
    public float monsterHunterExp { get; private set; }
    public int monsterHunterLevel { get; private set; }


    public void LevelUp(PlayerJobs job, float exp)
    {
        switch (job)
        {
            case PlayerJobs.Lumberjack:

                lumberjackExp += exp;
                if (LevelUpCheck(lumberjackExp))
                {
                    Debug.Log("Lumberjack Level Up");
                    lumberjackLevel++;
                    lumberjackExp = 0;
                }

                break;

            case PlayerJobs.Gatherer:

                gathererExp += exp;
                if (LevelUpCheck(gathererExp))
                {
                    Debug.Log("Gatherer Level Up");
                }

                break;

            case PlayerJobs.Hunter:

                hunterExp += exp;
                if (LevelUpCheck(hunterExp))
                {
                    Debug.Log("Hunter Level Up");
                }

                break;

            case PlayerJobs.MonsterHunter:

                monsterHunterExp += exp;
                if (LevelUpCheck(monsterHunterExp))
                {
                    Debug.Log("Monster Hunter Level Up");
                }

                break;
        }
    }

    bool LevelUpCheck(float currentExp)
    {
        bool levelUp = currentExp >= 100 ? true : false;
        return levelUp;
    }
}
