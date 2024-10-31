using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SoT.AbstractClasses;

public class DalamikGameManager : MonoSingleton<DalamikGameManager>
{
    [SerializeField]
    DalamikPlayerManager _playerManager;

    public GameTile startingGameTile;

    public List<string> playerNames = new List<string>();

    public List<DalamikPlayer>
        playerOrder = new List<DalamikPlayer>(),
        leaderboard = new List<DalamikPlayer>();

    [HideInInspector]
    public bool playerOrderRoll = true;

    public Dictionary<DalamikPlayer, int> playerStartingRoll = new Dictionary<DalamikPlayer, int>();

    public int currentPlayerTurn = 0;

    public int currentTurn;

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

            if (playerOrder[0].playerControls != null)
            {
                playerOrder[0].playerControls.ChangeTextDisplay("Roll");
                playerOrder[0].playerControls.gameTrigger.enabled = true;
            }

            else
                playerOrder[0].roll = true;
        }
    }

    public void NextPlayerTurn()
    {
        // checks to see if this is the last players turn
        if (currentPlayerTurn == 3)
            ActivateMiniGame();

        // sets game manager to next player
        else
        {
            currentPlayerTurn++;

            SetCurrentPlayerToBeAbleToRoll();

            UpdateLeaderboard();
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

        UpdateLeaderboard();

        currentTurn++;

        if (currentTurn == 30)
            GameOver();

        else
        {
            currentPlayerTurn = 0;

            SetCurrentPlayerToBeAbleToRoll();
        }
    }

    void SetCurrentPlayerToBeAbleToRoll()
    {
        playerOrder[currentPlayerTurn].canRoll = true;

        if (playerOrder[currentPlayerTurn].playerControls != null)
        {
            playerOrder[currentPlayerTurn].playerControls.ChangeTextDisplay("Roll");
            playerOrder[currentPlayerTurn].playerControls.gameTrigger.enabled = true;
        }

        else
            playerOrder[currentPlayerTurn].roll = true;
    }

    void GameOver()
    {
        Debug.Log("Give out winning rewards to players");

        for (int i = 0; i < leaderboard.Count; i++)
        {
            if (leaderboard[i].player != null)
            {
                switch (i)
                {
                    // 1st Place WINNER
                    case 0:

                        leaderboard[i].player.playerStats.AdjustGold(300);

                        break;

                    // 2nd Place
                    case 1:

                        leaderboard[i].player.playerStats.AdjustGold(100);

                        break;

                    // 3rd Place
                    case 2:

                        // Gets 50% off ticket for next game

                        break;

                    // 4th Place
                    case 3:

                        // Gets nothing

                        break;
                }
            }
        }

        // disable game controls
        foreach (PlayerController player in _playerManager.currentPlayers)
            player.leftHand.DalamikGameControls.SetActive(false);

        Debug.Log("Display Final Leaderboard, open portal back to real world");
    }

    public void UpdateLeaderboard()
    {
        // clears current leaderboard
        leaderboard.Clear();

        // create dictionary which will hold players and current total points
        Dictionary<DalamikPlayer, int> playerCurrentPoints = new Dictionary<DalamikPlayer, int>();

        // checks each players current currency and relics to determine total points then adds player and points to dictionary
        for (int i = 0; i < playerOrder.Count; i++)
        {
            int totalPoints = playerOrder[i].gameCurrency + (playerOrder[i].gameRelics * 10000);
            playerCurrentPoints.Add(playerOrder[i], totalPoints);
        }

        // sorts players based on their total points accumulated
        var sortedLeaderboard = playerCurrentPoints.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        // updates the leaderboard
        foreach (KeyValuePair<DalamikPlayer, int> pair in sortedLeaderboard)
            leaderboard.Add(pair.Key);

        // update player stat displays
        foreach (DalamikPlayer player in leaderboard)
            player.UpdateStatDisplay();
    }
}
