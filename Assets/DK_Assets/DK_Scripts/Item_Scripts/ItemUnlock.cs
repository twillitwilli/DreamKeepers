using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabable))]
public class ItemUnlock : MonoBehaviour
{
    GameItems _gameItems;

    public enum UnlockType
    {
        heartCrystal,
        sword,
        bow,
    }

    public UnlockType typeOfUnlock;

    [SerializeField]
    GameObject _unlockPrefab;

    private void Start()
    {
        _gameItems = DKGameManager.Instance.gameItems;
    }

    public void UnlockItem(VRHandController whichHand)
    {
        // Checks which item is being unlocked
        switch (typeOfUnlock)
        {
            case UnlockType.heartCrystal:

                HeartCrystalUpgrade();

                break;

            case UnlockType.sword:

                SwordUpgrade(whichHand);

                break;

            case UnlockType.bow:

                BowUpgrade(whichHand);

                break;
        }

        // destroy the unlock item after obtaining it
        Destroy(gameObject);
    }

    // --------------------- Upgrade Methods ----------------------------

    void HeartCrystalUpgrade()
    {
        PlayerStats.Instance.HealthCrystalObtained();
    }

    void SwordUpgrade(VRHandController hand)
    {
        // if you dont have sword unlocked
        if (!_gameItems.swords[0].itemUnlocked)
        {
            Debug.Log("Your 1st Sword!");
            _gameItems.swords[0].itemUnlocked = true;

            SpawnNewSword(hand);
        }

        // when you get a sword upgrade
        else
        {
            Debug.Log("Upgrading Sword");

            RemoveOldSword();

            SpawnNewSword(hand);
        }
    }

    void RemoveOldSword()
    {
        // Get old sword
        GameObject oldSword = PlayerEquipment.Instance.currentSword;

        // check if player is holding sword
        if (PlayerController.Instance.leftHand.currentEquippedItem.equipableType == GameItems.EquipableItem.sword)
        {
            // get left hand
            VRHandController leftHand = PlayerController.Instance.leftHand;

            // unequip sword from hand
            leftHand.UnequipItem();
        }

        else if (PlayerController.Instance.rightHand.currentEquippedItem.equipableType == GameItems.EquipableItem.sword)
        {
            // get right hand
            VRHandController rightHand = PlayerController.Instance.rightHand;

            // unequip sword from hand
            rightHand.UnequipItem();
        }

        Destroy(oldSword);
    }

    void SpawnNewSword(VRHandController hand)
    {
        // spawn new sword
        GameObject newSword = Instantiate(_unlockPrefab);

        // add sword to hand and realign
        hand.currentEquippedItem = newSword.GetComponent<EquipableItem>();
        newSword.transform.SetParent(hand.equippableItemSlot);
        newSword.GetComponent<EquipableItem>().PositionItem();

    }

    void BowUpgrade(VRHandController hand)
    {

    }

    // --------------------------------------------------------------------
}
