using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [System.Serializable]
    public class RankingData
    {
        public string name;
        public int score;
        
        public RankingData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
    public List<RankingData> data;

    public List<Text> rankerName;
    public List<Text> rankerScore;

    public GameObject registerUI;
    public InputField registerName;

    public GameObject titleBtn;

    public Text alertMessage;

    private void Start()
    {
        LoadData();
        SetRankerText();

        if (ScoreManager.instance == null)
        {
            Debug.LogError("스코어 매니저가 없습니다. 제대로 작동되지 않을 수 있습니다.");
            return;
        }

        if(ScoreManager.instance.finalScore == 0)
        {
            registerUI.SetActive(false);
            SetAlertMessage($"플레이 기록이 없습니다!");
            titleBtn.SetActive(true);
        }

        if (IsCanRegister(ScoreManager.instance.finalScore))
        {
            registerUI.SetActive(true);
            SetAlertMessage($"당신의 점수는 {ScoreManager.instance.finalScore:N0}점이므로 랭킹에 등록할 수 있습니다.");
        }
        else
        {
            registerUI.SetActive(false);
            SetAlertMessage($"당신의 점수는 {ScoreManager.instance.finalScore:N0}점이므로 랭킹에 등록할 수 없습니다.");
            titleBtn.SetActive(true);
        }
    }

    private void SortRanking()
    {
        data = data.OrderByDescending(a => a.score).ToList();

        if (data.Count >= 5)
            data = data.Take(5).ToList();
    }

    public bool IsCanRegister(int score)
    {
        if (data.Count < 5 || score > data.Last().score)
            return true;

        return false;
    }

    public void OnClickRegisterBtn()
    {
        if (string.IsNullOrEmpty(registerName.text))
        {
            SetAlertMessage("이름이 비어있습니다. 다시 확인해주세요.");
            return;
        }

        foreach(RankingData datas in data)
        {
            if (datas.name.Equals(registerName.text))
            {
                SetAlertMessage("이미 등록되어있는 이름입니다. 다시 확인해주세요.");
                return;
            }
        }

        foreach(char c in registerName.text.ToLower())
        {
            if (c < 'a' || c > 'z')
            {
                SetAlertMessage("영어가 아닌 부분이 존재합니다. 다시 확인해주세요.");
                return;
            }
        }

        RegisterRanking();
    }

    private void RegisterRanking()
    {
        registerUI.SetActive(false);
        titleBtn.SetActive(true);
        data.Add(new RankingData(registerName.text, ScoreManager.instance.finalScore));
        SetAlertMessage("등록되었습니다.");
        SaveData();
    }

    public void SaveData()
    {
        SortRanking();
        for(int i=0; i<data.Count; i++)
        {
            PlayerPrefs.SetString($"{i}Name", data[i].name);
            PlayerPrefs.SetInt($"{i}Score", data[i].score);
        }

        SetRankerText();
    }

    public void LoadData()
    {
        data = new();
        for (int i = 0; i < 5; i++)
        {
            string name = PlayerPrefs.GetString($"{i}Name", null);
            int score = PlayerPrefs.GetInt($"{i}Score", 0);

            if (string.IsNullOrEmpty(name) && score == 0)
                break;

            data.Add(new RankingData(name, score));
        }
    }

    private void SetRankerText()
    {
        for(int i=0; i<data.Count; i++)
        {
            rankerName[i].text = $"{data[i].name}";
            rankerScore[i].text = $"{data[i].score:N0}";
        }

        for(int i=data.Count; i<5; i++)
        {
            rankerName[i].text = "---";
            rankerScore[i].text = "------";
        }
    }

    public void OnClickTitleBtn()
    {
        SceneLoadManager.instance.SceneChange(SceneNames.Title);
        GetComponent<CanvasGroup>().interactable = false;
    }

    public void SetAlertMessage(string message)
    {
        alertMessage.text = message;
    }
}
