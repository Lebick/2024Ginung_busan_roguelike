using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayManager : Singleton<GamePlayManager>
{
    public bool isCutScene;
    public bool isPause;

    public PlayerController player;
    public List<EnemyController> enemys;

    public RectTransform overlayCanvas;
    public RectTransform indicatorCanvas;
    public Transform cameraCanvas;

    public Transform InstantiateObjectParent;
    public Transform enemyParent;
    public Transform[] enemySpawnPos;

    public Stage1Sequence stageSequence;

    public float currentRotation;

    public Transform rotationObj;

    public bool isEnemyStop;

    public float timer;

    public int[] enemyKillCount;
    public int getItemCount;

    public int deathCount;

    private void Start()
    {
        
    }

    private void Update()
    {
        enemys = enemyParent.GetComponentsInChildren<EnemyController>().ToList();
    }

    public void SetCutSceneState(bool value)
    {
        isCutScene = value;
    }

    public void GameOver(SaveData saveData)
    {
        DataSaveManager.instance.currentData = saveData;
        Invoke(nameof(Restart), 3f);
    }

    private void Restart()
    {
        SceneLoadManager.instance.SceneChange(SceneLoadManager.instance.currentScene);
    }
}
