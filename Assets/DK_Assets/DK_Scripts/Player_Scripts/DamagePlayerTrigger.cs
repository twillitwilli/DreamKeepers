using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerTrigger : MonoBehaviour
{
    [SerializeField]
    float _damage;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;

        if (other.gameObject.TryGetComponent<PlayerController>(out player))
        {
            player.playerStats.AdjustCurrentHealth(true, -_damage);
        }
    }
}
