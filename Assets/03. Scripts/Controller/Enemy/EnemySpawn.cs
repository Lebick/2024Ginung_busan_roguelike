using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;

    public float waitTime;

    private void Start()
    {
        Invoke(nameof(Spawn), waitTime);
    }

    public void SpawnBoss()
    {
        Instantiate(gameObject, GamePlayManager.instance.enemySpawnPos[0].position, Quaternion.identity);
    }

    private void Spawn()
    {
        Instantiate(enemy, transform.position, Quaternion.Euler(0, 0, GamePlayManager.instance.currentRotation), GamePlayManager.instance.enemyParent);
    }
}
