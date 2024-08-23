using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{

    public Text[] scoreUIs;
    public Text[] scoreValues;
    private List<int> addScoreList;
    public Text finalScore;

    public GameObject addScore;
    public GameObject subtractScore;
    public Transform addParent;
    private List<Animator> instantiateScores;

    public Animator[] enemySprite;
    public Text[] enemyKillCount;

    public Animator bossSprite;
    public Text bossKillCount;

    private int score = 0;

    private void Start()
    {
        SetScoreList();
        SetUI();
        StartCoroutine(StartResult());
    }

    private void SetScoreList()
    {
        addScoreList.Add(GamePlayManager.instance.player.level * ScoreManager.instance.levelUpScore);
        addScoreList.Add(GamePlayManager.instance.getItemCount * ScoreManager.instance.itemScore);
        addScoreList.Add(GamePlayManager.instance.player.hp * ScoreManager.instance.hpScore);
        addScoreList.Add(GamePlayManager.instance.player.mp * ScoreManager.instance.mpScore);
        addScoreList.Add(GamePlayManager.instance.deathCount * ScoreManager.instance.penaltyScore);
    }

    private void SetUI()
    {
        scoreValues[0].text = $"{GamePlayManager.instance.player.level}";
        scoreValues[1].text = $"{GamePlayManager.instance.getItemCount}";
        scoreValues[2].text = $"{GamePlayManager.instance.player.hp}";
        scoreValues[3].text = $"{GamePlayManager.instance.player.mp}";
        scoreValues[4].text = $"{GamePlayManager.instance.deathCount}";
    }

    private IEnumerator StartResult()
    {
        yield return new WaitForSeconds(2.25f);

        StartCoroutine(ScoreListAdd());
    }

    private IEnumerator ScoreListAdd()
    {
        for (int i = 0; i < scoreUIs.Length; i++)
        {
            float progress = 0f;
            Color myColor = scoreUIs[i].color;
            Color newColor = myColor;
            newColor.a = 1f;

            while (progress <= 1f)
            {
                progress += Time.deltaTime * 10f;
                Color currentColor = Color.Lerp(myColor, newColor, progress);
                scoreUIs[i].color = currentColor;
                scoreValues[i].color = currentColor;
                yield return null;
            }
            progress = 0;
            while (progress <= 1f)
            {
                progress += Time.deltaTime * 10f;
                Color currentColor = Color.Lerp(newColor, myColor, progress);
                scoreUIs[i].color = currentColor;
                scoreValues[i].color = currentColor;
                yield return null;
            }

            AddScoreListText(addScoreList[i]);

            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(CalculateScore(EnemyScore()));
    }

    //스코어 텍스트를 등록
    private void AddScoreListText(int value)
    {
        Text score;

        if (value != 0)
        {
            if (value > 0)
                score = Instantiate(addScore, addParent).GetComponentInChildren<Text>();
            else
                score = Instantiate(subtractScore, addParent).GetComponentInChildren<Text>();

            score.text = $"{value}";

            instantiateScores.Add(score.GetComponent<Animator>());
        }
    }

    //정산
    private IEnumerator CalculateScore(IEnumerator endEvent = null)
    {
        while(instantiateScores.Count > 0)
        {
            int previousScore = score;
            int newScore = score + int.Parse(instantiateScores[0].GetComponent<Text>().text);

            instantiateScores[0].SetTrigger("Delete");

            yield return new WaitForSeconds(0.5f);

            Destroy(instantiateScores[0].transform.parent.gameObject);
            instantiateScores.RemoveAt(0);

            float progress = 0f;

            while (progress <= 1f)
            {
                progress += Time.deltaTime * 2f;
                score = (int)Mathf.Lerp(previousScore, newScore, progress);
                finalScore.text = $"{score}";
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.5f);

        if(endEvent != null)
            StartCoroutine(endEvent);
    }

    private IEnumerator EnemyScore()
    {
        for(int i=0; i<enemySprite.Length; i++)
        {
            int killCount = GamePlayManager.instance.enemyKillCount[i];
            enemySprite[i].SetTrigger("Show");
            yield return new WaitForSeconds(0.33f);
            
            for(int j=0; j<killCount; j++)
            {
                enemyKillCount[i].text = $"x{j}";
                yield return new WaitForSeconds(0.2f);
            }

            AddScoreListText(killCount * ScoreManager.instance.killScore);
        }
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(CalculateScore(BossScore()));
    }

    private IEnumerator BossScore()
    {
        bossSprite.SetTrigger("Show");
        yield return new WaitForSeconds(0.33f);

        bossKillCount.text = $"x1";
        yield return new WaitForSeconds(0.2f);

        AddScoreListText(ScoreManager.instance.bossScore);
        StartCoroutine(CalculateScore());
    }

    public void OnClickNextStage()
    {
        int currentScene = SceneLoadManager.instance.currentScene;

        SceneLoadManager.instance.SceneChange((currentScene + 1));
            
    }
}
