using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public enum ItemType
    {
        gold,
        arrow,
        lifeDrop,
        manaDrop,
        lifeCrystal,
        manaCrystal
    }

    [SerializeField]
    ItemType _dropType;

    [SerializeField]
    float _itemValue;

    [SerializeField]
    GameObject _dropPrefabParent;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;
        VRHandController hand;

        // if player hand or body touches the drop, it will check to see which type of drop this is and give it to the player
        if (other.gameObject.TryGetComponent<PlayerController>(out player) || other.gameObject.TryGetComponent<VRHandController>(out hand))
        {
            // gets the PlayerStats component which is where the data is stored for drops
            PlayerStats playerStats = PlayerController.Instance.playerStats;

            switch (_dropType)
            {
                // Gold Drop
                case ItemType.gold:

                    playerStats.AdjustGold(Mathf.RoundToInt(_itemValue));

                    break;

                // Arrow Drop
                case ItemType.arrow:

                    playerStats.AdjustArrows(Mathf.RoundToInt(_itemValue));

                    break;

                // Life Drop
                case ItemType.lifeDrop:

                    playerStats.AdjustCurrentHealth(false, _itemValue);

                    break;

                // Mana Drop
                case ItemType.manaDrop:

                    Debug.Log("Mana Drop Not Configured");

                    break;

                // Life Crystal Drop
                case ItemType.lifeCrystal:

                    playerStats.HealthCrystalObtained();

                    break;

                // Mana Crystal Drop
                case ItemType.manaCrystal:

                    Debug.Log("Mana Crystal Drop Not Configured");

                    break;
            }

            // After Drop is given to player, destroy this 
            Destroy(_dropPrefabParent);
        }
    }
}
