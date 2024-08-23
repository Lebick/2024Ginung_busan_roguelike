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
            Debug.LogError("���ھ� �Ŵ����� �����ϴ�. ����� �۵����� ���� �� �ֽ��ϴ�.");
            return;
        }

        if(ScoreManager.instance.finalScore == 0)
        {
            registerUI.SetActive(false);
            SetAlertMessage($"�÷��� ����� �����ϴ�!");
            titleBtn.SetActive(true);
        }

        if (IsCanRegister(ScoreManager.instance.finalScore))
        {
            registerUI.SetActive(true);
            SetAlertMessage($"����� ������ {ScoreManager.instance.finalScore:N0}���̹Ƿ� ��ŷ�� ����� �� �ֽ��ϴ�.");
        }
        else
        {
            registerUI.SetActive(false);
            SetAlertMessage($"����� ������ {ScoreManager.instance.finalScore:N0}���̹Ƿ� ��ŷ�� ����� �� �����ϴ�.");
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
            SetAlertMessage("�̸��� ����ֽ��ϴ�. �ٽ� Ȯ�����ּ���.");
            return;
        }

        foreach(RankingData datas in data)
        {
            if (datas.name.Equals(registerName.text))
            {
                SetAlertMessage("�̹� ��ϵǾ��ִ� �̸��Դϴ�. �ٽ� Ȯ�����ּ���.");
                return;
            }
        }

        foreach(char c in registerName.text.ToLower())
        {
            if (c < 'a' || c > 'z')
            {
                SetAlertMessage("��� �ƴ� �κ��� �����մϴ�. �ٽ� Ȯ�����ּ���.");
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
        SetAlertMessage("��ϵǾ����ϴ�.");
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
