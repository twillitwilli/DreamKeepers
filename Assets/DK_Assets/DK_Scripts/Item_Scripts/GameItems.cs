using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItems : MonoBehaviour
{
    public enum EquipableItem
    {
        sword,
        bow,
        questScroll
    }

    // Item Unlocks
    public bool
        magicGlove,
        fireCrystal,
        iceCrystal;

    public List<Item> swords;

    public List<Item> bows;

    public List<Item> keys;
}
