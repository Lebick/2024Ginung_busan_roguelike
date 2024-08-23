using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    public List<Transform> enemySpawnPos; //적이 스폰될 위치들
    public List<GameObject> enemys; //적들
    public GameObject enemySpawnEffect; //적이 생성되는 이펙트

    public float defaultSpawnCycle; //기본적으로 생성에 필요한 시간
    public int defaultSpawnCount; //기본적으로 한번 소환할 때 생성될 적의 수

    public float currentSpawnCycle;
    public int currentSpawnCount;

    public float decreaseSpawnCycle; //난이도가 상승할 때 줄어들 스폰주기
    public int increaseSpawnCount; //난이도가 상승할 때 상승할 스폰수

    private bool isSpawn = false;

    private void Start()
    {
        currentSpawnCycle = defaultSpawnCycle;
        currentSpawnCount = defaultSpawnCount;

        StartCutSceneEnd();
        //GamePlayManager.instance.stageSequence.startSequenceEnd.Add(StartCutSceneEnd);
    }

    private void StartCutSceneEnd()
    {
        isSpawn = true;
        InvokeRepeating(nameof(SpawnEnemy), currentSpawnCycle, currentSpawnCycle);
    }

    private void Update()
    {
        if (!isSpawn) return;
        if (GamePlayManager.instance.isCutScene || GamePlayManager.instance.isPause) return;

        GamePlayManager.instance.timer += Time.deltaTime;
        if(GamePlayManager.instance.timer >= 120f)
        {
            isSpawn = false;
            CancelInvoke(nameof(SpawnEnemy));
            GamePlayManager.instance.stageSequence.BossSequence();
            Destroy(gameObject);
        }
        else if(!IsInvoking(nameof(SpawnEnemy)))
        {
            InvokeRepeating(nameof(SpawnEnemy), 0, currentSpawnCycle);
        }
    }

    public void SequenceSpawn()
    {
        List<Transform> copySpawnPos = new(enemySpawnPos);
        copySpawnPos = copySpawnPos.OrderByDescending(a => Vector3.Distance(GamePlayManager.instance.player.transform.position, a.position)).ToList();
        for (int i = 0; i < currentSpawnCount; i++)
        {
            int randomEnemy = Random.Range(0, enemys.Count);
            EnemySpawn enemy = Instantiate(enemySpawnEffect, copySpawnPos[i].position, Quaternion.identity, GamePlayManager.instance.enemyParent)
                .GetComponent<EnemySpawn>();

            enemy.enemy = enemys[randomEnemy];
        }
    }

    private void SpawnEnemy()
    {
        List<Transform> copySpawnPos = new(enemySpawnPos);
        copySpawnPos = copySpawnPos.OrderBy(a => Vector3.Distance(GamePlayManager.instance.player.transform.position, a.position)).ToList();
        for(int i=0; i<currentSpawnCount; i++)
        {
            int randomEnemy = Random.Range(0, enemys.Count);
            EnemySpawn enemy = Instantiate(enemySpawnEffect, copySpawnPos[i].position, Quaternion.identity, GamePlayManager.instance.enemyParent)
                .GetComponent<EnemySpawn>();

            enemy.enemy = enemys[randomEnemy];
        }

        currentSpawnCycle -= decreaseSpawnCycle;
        currentSpawnCount += increaseSpawnCount;
    }
}
