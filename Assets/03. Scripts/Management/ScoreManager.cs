using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : Singleton<ScoreManager>
{
    public readonly int killScore = 500;
    public readonly int bossScore = 20000;
    public readonly int levelUpScore = 5000;
    public readonly int hpScore = 10;
    public readonly int mpScore = 10;
    public readonly int itemScore = 500;
    public readonly int penaltyScore = -20000;

    public int finalScore;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title")
            Reset();
    }

    private void Reset()
    {
        finalScore = 0;
    }
}
