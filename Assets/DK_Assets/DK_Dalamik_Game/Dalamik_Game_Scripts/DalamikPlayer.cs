using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DalamikPlayer : MonoBehaviour
{
    public string playerName;

    public bool
        canRoll,
        roll,
        miniGameBlueTeam;

    public int
        maxRollValue = 12,
        lastRoll,
        spacesCanMove,
        gameCurrency = 20,
        gameRelics;

    [HideInInspector]
    public GameTile currentTile;

    private async void Start()
    {
        // wait random time before setting player base order
        await Task.Delay(Random.Range(500, 1500));

        // set player name to game manager
        DalamikGameManager.Instance.playerNames.Add(playerName);

        // set starting tile
        currentTile = DalamikGameManager.Instance.startingGameTile;
    }

    private void Update()
    {
        // checks to see if player can roll and has rolled
        if (canRoll && roll)
            Roll();
    }

    public void Roll()
    {
        // disables can roll and roll
        canRoll = false;
        roll = false;

        // if this is the first roll of the game
        if (DalamikGameManager.Instance.playerOrderRoll)
        {
            // selects random value between 1 and 1000
            int selectedRollValue = Random.Range(1, 1000);

            // sends player and random value to game manager to determine player order of the game
            DalamikGameManager.Instance.GetPlayerOrder(this, selectedRollValue);
        }

        else
        {
            // random movement spaces count
            int selectedRollValue = Random.Range(1, maxRollValue);

            // set how many spaces player can move
            spacesCanMove = selectedRollValue;

            // sets the last amount of spaces the player can move
            lastRoll = selectedRollValue;
        }
    }

    public void PlayerMovedASpace(GameTile spacedPlayerMovedTo)
    {
        // sets the current tile the player is at
        currentTile = spacedPlayerMovedTo;

        // removes 1 movement space from how many spaces the player can move
        spacesCanMove--;

        // if the player cant move anymore, activates next players turn
        if (spacesCanMove == 0)
            DalamikGameManager.Instance.NextPlayerTurn();
    }
}
