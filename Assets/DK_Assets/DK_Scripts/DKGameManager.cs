using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class DKGameManager : MonoSingleton<DKGameManager>
{
    public GameItems gameItems;

    // Portal Nexus Unlocks
    [HideInInspector]
    public bool
        namikVillagePortal,
        lakeVillagePortal,
        outsideCastlePortal,
        volcanoVillagePortal,
        iceVillagePortal,
        wizardTownPortal;
}
