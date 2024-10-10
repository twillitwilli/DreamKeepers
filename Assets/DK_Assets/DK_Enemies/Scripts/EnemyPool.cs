using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class EnemyPool : PoolManager<EnemyPool>
{
    public EnemyData enemyData;

    [SerializeField]
    public int clusterNumber;

    public override void Awake()
    {
        
    }
}
