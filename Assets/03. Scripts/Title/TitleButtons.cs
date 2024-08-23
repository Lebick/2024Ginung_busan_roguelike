using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public CanvasGroupSetting introduceCanvas;
    public CanvasGroupSetting howToPlayCanvas;

    private void Start()
    {
        canvasGroup= GetComponent<CanvasGroup>();
    }

    private void SetInteractable(bool value)
    {
        canvasGroup.interactable = value;
    }

    public void _0_OnClickStartBtn()
    {
        print("시작");
        SetInteractable(false);
        SceneLoadManager.instance.SceneChange(SceneNames.Stage1);
    }

    public void _1_OnClickIntroduceBtn()
    {
        print("소개");
        SetInteractable(false);
        introduceCanvas.gameObject.SetActive(true);
        introduceCanvas.Alpha0to1(0.5f);
    }

    public void _2_OnClickHowtoPlayBtn()
    {
        print("플레이 방법");
        SetInteractable(false);
        howToPlayCanvas.gameObject.SetActive(true);
        howToPlayCanvas.Alpha0to1(0.5f);
    }

    public void _3_OnClickRankingBtn()
    {
        SceneLoadManager.instance.SceneChange(SceneNames.Ranking);
        SetInteractable(false);
    }

    public void _4_OnClickExitBtn()
    {
        Application.Quit();
        SetInteractable(false);
    }

    public void _8_OnClickIntroduceExitBtn()
    {
        introduceCanvas.Alpha1to0(0.5f, () =>
        {
            introduceCanvas.gameObject.SetActive(false);
            SetInteractable(true);
        });
    }

    public void _9_OnClickHowToPlayExitBtn()
    {
        howToPlayCanvas.Alpha1to0(0.5f, () =>
        {
            howToPlayCanvas.gameObject.SetActive(false);
            SetInteractable(true);
        });
    }
}
