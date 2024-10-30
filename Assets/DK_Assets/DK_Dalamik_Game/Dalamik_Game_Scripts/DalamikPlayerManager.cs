using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class DalamikPlayerManager : MonoSingleton<DalamikGameManager>
{
    public DalamikPlayer[] playerTrackers;

    public List<PlayerController> currentPlayers = new List<PlayerController>();

    int _assignedPlayer;

    private void Start()
    {
        // when playing alone
        currentPlayers.Add(PlayerController.Instance);
        AssignPlayers();
    }

    void AssignPlayers()
    {
        for (int i = 0; i < currentPlayers.Count; i++)
        {
            // turns off AI for this tracker
            playerTrackers[i].isAI = false;

            // set player to player tracker
            playerTrackers[i].player = currentPlayers[i];

            // set game controls to player tracker
            playerTrackers[i].playerControls = currentPlayers[i].leftHand.DalamikGameControls.GetComponent<DalamikPlayerControls>();

            // turn on game control on player
            currentPlayers[i].leftHand.DalamikGameControls.SetActive(true);

            // turn on current tile display
            playerTrackers[i].currentSpaceDisplayer.SetActive(true);

            // set dalamik player to player controls
            playerTrackers[i].playerControls.dalamikPlayer = playerTrackers[i];

            // set text display
            playerTrackers[i].playerControls.gameTrigger.enabled = true;
            playerTrackers[i].playerControls.ChangeTextDisplay("Roll");
        }
    }
}
