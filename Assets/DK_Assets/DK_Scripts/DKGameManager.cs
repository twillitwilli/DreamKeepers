using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class DKGameManager : MonoSingleton<DKGameManager>
{
    public GameItems gameItems;

    public int spawnLocation { get; set; } = 0;

    public override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        base.Awake();
    }
}
