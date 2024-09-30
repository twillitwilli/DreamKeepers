using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    PlayerController _playerController;

    public Transform
        body,
        head,
        leftHand,
        rightHand;

    public Renderer[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _playerController = PlayerController.Instance;

        // Disable the meshes for the owner, because owner doesnt need to see there own network components
        if (IsOwner)
        {
            foreach (var mesh in meshToDisable)
                mesh.enabled = false;
        }
    }

    private void Update()
    {
        if (IsOwner)
            SyncPlayerMovements();
    }

    void SyncPlayerMovements()
    {
        // Sync Network body to player body
        body.position = _playerController.transform.position;
        body.rotation = _playerController.transform.rotation;
        
        // Sync Network head to player head
        head.position = _playerController.head.position;
        head.rotation = _playerController.head.rotation;

        // Sync Network left hand to player left hand
        leftHand.position = _playerController.leftHand.transform.position;
        leftHand.rotation = _playerController.leftHand.transform.rotation;

        // Sync Network right hand to player right hand
        rightHand.position = _playerController.rightHand.transform.position;
        rightHand.rotation = _playerController.rightHand.transform.rotation;
    }
}
