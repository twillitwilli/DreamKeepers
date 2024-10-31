using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    public enum TypesOfTiles
    {
        normal,
        cursed,
        prize,
        specialSpace
    }

    public TypesOfTiles tileType;

    public bool isStartingTile;

    public GameTile[]
        nextTile,
        previousTile;

    public void ActivateTile(DalamikPlayer player, bool playerStopped = false)
    {
        switch (tileType)
        {
            case TypesOfTiles.normal:

                player.gameCurrency += 6;

                player.miniGameBlueTeam = true;

                break;

            case TypesOfTiles.cursed:

                player.gameCurrency -= 6;

                if (player.gameCurrency < 0)
                    player.gameCurrency = 0;

                player.miniGameBlueTeam = false;

                break;

            case TypesOfTiles.prize:

                Debug.Log("Can Get Prize");

                if (player.gameCurrency > 50)
                    player.gameRelics++;

                if (playerStopped)
                    player.miniGameBlueTeam = RandomizeMiniGameTeam();

                break;

            case TypesOfTiles.specialSpace:

                Debug.Log("Passed A Special Space");

                if (playerStopped)
                    player.miniGameBlueTeam = RandomizeMiniGameTeam();

                break;
        }
    }

    bool RandomizeMiniGameTeam()
    {
        bool randomTeam = Random.Range(0, 1) == 1 ? true : false;

        return randomTeam;
    }
}
