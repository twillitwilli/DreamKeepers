using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public GameTile gameTile;

    private void OnTriggerEnter(Collider other)
    {
        DalamikPlayer player;

        // checks to see if player that touched trigger is the current player that should be moving
        if (other.gameObject.TryGetComponent<DalamikPlayer>(out player) && player == DalamikGameManager.Instance.playerOrder[DalamikGameManager.Instance.currentPlayerTurn])
        {
            // checks to see if the tile the player is trying to move to is on the path from the last time the player was at
            if (player.currentTile.nextTile[0] == gameTile)
                MoveToThisSpace(player);

            else if (player.currentTile.nextTile.Length > 1 && player.currentTile.nextTile[1] == gameTile)
                MoveToThisSpace(player);
        }
    }

    void MoveToThisSpace(DalamikPlayer player)
    {
        // player moved to this space
        player.PlayerMovedASpace(gameTile);

        // if player stopped at this space, activate tile functions
        if (player.spacesCanMove == 0)
            gameTile.ActivateTile(player, true);

        // if player passes prize or special tile, player can access this tiles function
        else if (gameTile.tileType == GameTile.TypesOfTiles.prize || gameTile.tileType == GameTile.TypesOfTiles.specialSpace)
            gameTile.ActivateTile(player);
    }
}
