using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using SoT.AbstractClasses;
using SoT.Interfaces;

public class DKEnemySpawner : MonoSingleton<DKEnemySpawner>, iCooldownable
{
    public bool canSpawnEnemies { get; set; }

    [SerializeField]
    EnemyPool[] _spawnableEnemies;

    public float cooldownTimer { get; set; }

    public int currentEnemyCount { get; set; }

    int _clusterChance;

    private async void Start()
    {
        // wait 10 seconds
        await Task.Delay(10000);

        canSpawnEnemies = true;
    }

    private void Update()
    {
        if (canSpawnEnemies && CooldownDone(false, 0))
            SpawnEnemies();
    }

    public void SpawnEnemies(bool randomEnemy = true, int specificEnemy = 0, bool clusterSpawn = false)
    {
        int randomSpawnChance = Random.Range(0, 100);

        // Dont Spawn Enemies If Conditions Not Met
        if (currentEnemyCount >= 50 || randomSpawnChance < 40)
        {
            // resets cooldown time before spawn can occur again
            CooldownDone(true, Random.Range(60, 180));
            return;
        }

        // Spawn Enemy
        else
        {
            // get new enemy
            int whichEnemy = randomEnemy ? GetRandomSpawn() : specificEnemy;
            GameObject newEnemy = _spawnableEnemies[whichEnemy].GetItem();

            // new enemy postiion
            //newEnemy.transform.position
            //newEnemy.transform.rotation

            // set enemy active
            newEnemy.SetActive(true);

            // get enemy controller
            DKEnemyController enemyController = newEnemy.GetComponent<DKEnemyController>();

            // adjust enemy controller stats
            enemyController.enemyStats = _spawnableEnemies[whichEnemy].enemyData;

            // check if enemy can cluster
            if (!clusterSpawn)
            {
                _clusterChance = _spawnableEnemies[whichEnemy].clusterNumber;
                SpawnEnemies(false, whichEnemy, true);
                return;
            }

            // if this is a cluster enemy spawn
            else
            {
                _clusterChance--;

                if (_clusterChance > 0)
                {
                    SpawnEnemies(false, whichEnemy, true);
                }    
            }

            // resets cooldown time before spawn can occur again
            CooldownDone(true, Random.Range(60, 180));
        }
    }

    int GetRandomSpawn()
    {
        return Random.Range(0, _spawnableEnemies.Length);
    }

    public bool CooldownDone(bool setTimer, float cooldownTime)
    {
        if (setTimer)
        {
            cooldownTimer = cooldownTime;
            setTimer = false;
        }

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        else if (cooldownTimer <= 0)
            return true;

        return false;
    }
}
