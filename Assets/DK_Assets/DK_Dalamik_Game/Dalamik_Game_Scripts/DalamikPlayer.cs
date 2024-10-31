using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DalamikPlayer : MonoBehaviour
{
    public string playerName;

    public GameObject currentSpaceDisplayer;

    public bool
        canRoll,
        roll,
        miniGameBlueTeam,
        isAI = true;

    public int
        maxRollValue = 12,
        lastRoll,
        spacesCanMove,
        gameCurrency = 20,
        gameRelics;

    [SerializeField]
    Text
        playerNameText,
        tokensText,
        relicsText,
        lastRollText,
        playerLeaderboardPosText;

    public GameTile currentTile { get; set; }
    public PlayerController player { get; set; }
    public DalamikPlayerControls playerControls { get; set; }

    bool _AICanMove;
    Vector3 _AITargetPos;

    private async void Start()
    {
        // wait random time before setting player base order
        await Task.Delay(Random.Range(500, 1500));

        // set player name to game manager
        DalamikGameManager.Instance.playerNames.Add(playerName);

        // update player name display
        playerNameText.text = playerName;

        // set starting tile
        currentTile = DalamikGameManager.Instance.startingGameTile;

        // wait 3 - 5 seconds
        await Task.Delay(Random.Range(3000, 5000));

        if (isAI)
            roll = true;
    }

    private void Update()
    {
        Roll();

        if (_AICanMove)
        {
            if (isAI)
                AIMovementOnBoard();

            else
                FollowPlayer();
        }

        if (isAI && _AICanMove)
        {
            AIMovementOnBoard();
        }
    }

    public void Roll()
    {
        // checks to see if player can roll and has rolled
        if (canRoll && roll)
        {
            // disables can roll and roll
            canRoll = false;
            roll = false;

            // if this is the first roll of the game
            if (DalamikGameManager.Instance.playerOrderRoll)
            {
                // selects random value between 1 and 1000
                int selectedRollValue = Random.Range(1, 1000);

                // displays roll value
                if (player != null)
                    playerControls.ChangeTextDisplay("Roll " + selectedRollValue);

                lastRoll = selectedRollValue;

                // sends player and random value to game manager to determine player order of the game
                DalamikGameManager.Instance.GetPlayerOrder(this, selectedRollValue);
            }

            else
            {
                // random movement spaces count
                int selectedRollValue = Random.Range(1, maxRollValue);

                // displays roll value
                if (player != null)
                    playerControls.ChangeTextDisplay("Roll " + selectedRollValue);

                // set how many spaces player can move
                spacesCanMove = selectedRollValue;

                // sets the last amount of spaces the player can move
                lastRoll = selectedRollValue;

                // sets ai movement position
                _AITargetPos = GetNextSpacePosition();

                // lets AI Move on board
                _AICanMove = true;
            }
        }
    }

    public void PlayerMovedASpace(GameTile spacedPlayerMovedTo)
    {
        // sets the current tile the player is at
        currentTile = spacedPlayerMovedTo;

        // removes 1 movement space from how many spaces the player can move
        spacesCanMove--;

        if (player != null)
        {
            // display roll value
            playerControls.ChangeTextDisplay("Move " + spacesCanMove);

            // move current tile effect
            currentSpaceDisplayer.transform.position = currentTile.transform.position;
        }

        // if the player cant move anymore, activates next players turn
        if (spacesCanMove == 0)
        {
            _AICanMove = false;
            DalamikGameManager.Instance.NextPlayerTurn();
        }

        else if (isAI)
        {
            _AITargetPos = GetNextSpacePosition();
        }
    }

    Vector3 GetNextSpacePosition()
    {
        int movementTile = 0;

        if (currentTile.nextTile.Length > 1)
            movementTile = Random.Range(0, currentTile.nextTile.Length);

        Vector3 newPostion = new Vector3(currentTile.nextTile[movementTile].transform.position.x, transform.position.y, currentTile.nextTile[movementTile].transform.position.z);

        return newPostion;
    }

    void AIMovementOnBoard()
    {
        transform.position = Vector3.MoveTowards(transform.position, _AITargetPos, 5 * Time.deltaTime);
    }

    void FollowPlayer()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, playerPos, 5 * Time.deltaTime);
    }

    public void UpdateStatDisplay()
    {
        tokensText.text = "Tokens " + gameCurrency;
        relicsText.text = "Relics " + gameRelics;
        lastRollText.text = "Last Roll " + lastRoll;

        switch (DalamikGameManager.Instance.leaderboard.IndexOf(this))
        {
            case 0:

                playerLeaderboardPosText.text = "1st";

                break;

            case 1:

                playerLeaderboardPosText.text = "2nd";

                break;

            case 2:

                playerLeaderboardPosText.text = "3rd";

                break;

            case 3:

                playerLeaderboardPosText.text = "4th";

                break;
        }
    }
}
