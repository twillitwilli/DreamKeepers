using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItems : MonoBehaviour
{
    // Item Unlocks
    [HideInInspector]
    public bool
        sword,
        bow,
        magicGlove,
        fireCrystal,
        iceCrystal;

    public List<Item> Keys;
}
