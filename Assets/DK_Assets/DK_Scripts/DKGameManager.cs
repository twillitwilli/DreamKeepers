using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class DKGameManager : MonoSingleton<DKGameManager>
{
    public GameItems gameItems;

    public int spawnLocation { get; set; } = 0;
    public string nameOfCurrentArea { get; set; }
    public float currentGameTime { get; set; }

    public override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        base.Awake();
    }
}
