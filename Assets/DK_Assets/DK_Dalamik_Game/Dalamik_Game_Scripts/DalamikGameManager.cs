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
        // add player and roll to starting roll dictionary
        playerStartingRoll.Add(player, startingRoll);

        // checks to see if player starting roll dictionary is equal to the number of players
        if (playerStartingRoll.Count == 4)
        {
            // sorts the players by descending order based on their starting rolls
            var sortedPlayerOrder = playerStartingRoll.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            // adds each player to the player order that was previously sorted
            foreach (KeyValuePair<DalamikPlayer, int> pair in sortedPlayerOrder)
                playerOrder.Add(pair.Key);

            // disable player order roll
            playerOrderRoll = false;

            // enable 1st player to let the start the game and roll
            playerOrder[0].canRoll = true;
        }
    }

    public void NextPlayerTurn()
    {
        // checks to see if this is the last players turn
        if (currentPlayerTurn == 3)
        {
            // activate mini game, and reset the current player turn to the 1st player
            currentPlayerTurn = 0;
            ActivateMiniGame();
        }

        // sets game manager to next player
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
