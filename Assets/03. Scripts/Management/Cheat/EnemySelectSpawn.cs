using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectSpawn : MonoBehaviour
{
    private Transform enemySpawnPos;
    public GameObject[] enemys;
    public GameObject enemySpawnEffect;

    private void Start()
    {
        enemySpawnPos = GamePlayManager.instance.enemySpawnPos[0];
    }

    public void OnClickSpawnButton(int index)
    {
        EnemySpawn enemy = Instantiate(enemySpawnEffect, enemySpawnPos.position, enemys[index].transform.rotation, GamePlayManager.instance.enemyParent)
            .GetComponent<EnemySpawn>();

        enemy.enemy = enemys[index];
        Destroy(gameObject);
    }
}
