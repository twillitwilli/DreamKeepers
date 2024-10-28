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
        if (canRoll && roll)
            Roll();
    }

    public void Roll()
    {
        canRoll = false;
        roll = false;

        if (DalamikGameManager.Instance.playerOrderRoll)
        {
            int selectedRollValue = Random.Range(1, 1000);

            DalamikGameManager.Instance.GetPlayerOrder(this, selectedRollValue);
        }

        else
        {
            int selectedRollValue = Random.Range(1, maxRollValue);

            spacesCanMove = selectedRollValue;

            lastRoll = selectedRollValue;
        }
    }

    public void PlayerMovedASpace(GameTile spacedPlayerMovedTo)
    {
        currentTile = spacedPlayerMovedTo;

        spacesCanMove--;

        if (spacesCanMove == 0)
            DalamikGameManager.Instance.NextPlayerTurn();
    }
}
