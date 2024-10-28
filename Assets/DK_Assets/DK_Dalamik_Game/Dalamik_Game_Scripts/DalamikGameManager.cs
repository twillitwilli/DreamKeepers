using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SoT.AbstractClasses;

public class DalamikGameManager : MonoSingleton<DalamikGameManager>
{
    public GameTile startingGameTile;

    public string[]
        leaderboard;

    public List<string> playerNames = new List<string>();

    public List<DalamikPlayer> playerOrder = new List<DalamikPlayer>();

    [HideInInspector]
    public bool playerOrderRoll = true;

    public Dictionary<DalamikPlayer, int> playerStartingRoll = new Dictionary<DalamikPlayer, int>();

    public int currentPlayerTurn;

    public void GetPlayerOrder(DalamikPlayer player, int startingRoll)
    {
        playerStartingRoll.Add(player, startingRoll);

        if (playerStartingRoll.Count == 4)
        {
            var sortedPlayerOrder = playerStartingRoll.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<DalamikPlayer, int> pair in sortedPlayerOrder)
                playerOrder.Add(pair.Key);

            playerOrderRoll = false;
            playerOrder[0].canRoll = true;
        }
    }

    public void NextPlayerTurn()
    {
        if (currentPlayerTurn == 3)
        {
            currentPlayerTurn = 0;
            ActivateMiniGame();
        }

        else
        {
            currentPlayerTurn++;
            playerOrder[currentPlayerTurn].canRoll = true;
        }
    }

    void ActivateMiniGame()
    {
        Debug.Log("Mini Game Start");

        MiniGameEnd();
    }

    public void MiniGameEnd()
    {
        Debug.Log("Mini Game End");

        playerOrder[0].canRoll = true;
    }
}
