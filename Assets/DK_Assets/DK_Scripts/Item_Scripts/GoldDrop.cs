using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : MonoBehaviour
{
    [SerializeField]
    int _goldAmount;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;
        VRHandController hand;

        if (other.gameObject.TryGetComponent<PlayerController>(out player) || other.gameObject.TryGetComponent<VRHandController>(out hand))
        {
            PlayerController.Instance.playerStats.AdjustGold(_goldAmount);
            Destroy(gameObject);
        }
    }
}
