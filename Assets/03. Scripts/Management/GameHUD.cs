using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : Singleton<GameHUD>
{
    private bool cutSceneState;
    private CanvasGroupSetting canvasGroup;

    public Text bossLimitText;
    public Text scoreText;

    public CanvasGroupSetting bossCanvasGroup;
    public Image bossHPValue;
    public Image boss3HPValue;

    public Transform alertParent;
    public GameObject alertMessage;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroupSetting>();
    }
    
    public void Update()
    {
        UpdateCanvasGroup();

        UpdateTime(GamePlayManager.instance);
        UpdateScore(ScoreManager.instance);
    }

    public void SpawnAlertMessage(string msg, Color bgColor)
    {
        GameObject alert = Instantiate(alertMessage, alertParent);

        Image alertBG = alert.GetComponentInChildren<Image>();
        alertBG.color = bgColor;

        Text alertText = alert.GetComponentInChildren<Text>();
        alertText.text = msg;

        alert.transform.SetAsFirstSibling();

        Destroy(alert, 2f);
    }

    public void UpdateBossHPValue(float maxValue, float currentValue)
    {
        bossHPValue.fillAmount = currentValue / maxValue;
    }
    public void UpdateBoss3HPValue(float maxValue, float currentValue)
    {
        boss3HPValue.fillAmount = currentValue / maxValue;
    }

    private void UpdateCanvasGroup()
    {
        if (GamePlayManager.instance.isCutScene != cutSceneState)
        {
            cutSceneState = GamePlayManager.instance.isCutScene;

            if (cutSceneState)
                canvasGroup.Alpha1to0();
            else
                canvasGroup.Alpha0to1();
        }
    }

    private void UpdateTime(GamePlayManager manager)
    {
        int time = 120 - (int)manager.timer;
        int mm = time / 60;
        int ss = time - mm * 60;
        bossLimitText.text = $"Limit | {mm:D2}:{ss:D2}";
    }

    private void UpdateScore(ScoreManager manager)
    {
        scoreText.text = $"Score | {manager.finalScore:N0}";
    }
}