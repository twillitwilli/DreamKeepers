using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public GameTile gameTile;

    private void OnTriggerEnter(Collider other)
    {
        DalamikPlayer player;

        if (other.gameObject.TryGetComponent<DalamikPlayer>(out player) && player == DalamikGameManager.Instance.playerOrder[DalamikGameManager.Instance.currentPlayerTurn])
        {
            if (player.currentTile.nextTile[0] == gameTile)
                MoveToThisSpace(player);

            else if (player.currentTile.nextTile.Length > 1 && player.currentTile.nextTile[1] == gameTile)
                MoveToThisSpace(player);
        }
    }

    void MoveToThisSpace(DalamikPlayer player)
    {
        player.PlayerMovedASpace(gameTile);

        if (player.spacesCanMove == 0)
            gameTile.ActivateTile(player, true);

        else if (gameTile.tileType == GameTile.TypesOfTiles.prize || gameTile.tileType == GameTile.TypesOfTiles.specialSpace)
            gameTile.ActivateTile(player);
    }
}
