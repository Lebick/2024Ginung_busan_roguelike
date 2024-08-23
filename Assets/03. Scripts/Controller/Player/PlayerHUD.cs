using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private CanvasGroupSetting canvasGroup;

    private bool cutSceneState;

    public Text alertText;
    public CanvasGroupSetting alertTextCanvas;

    public Image hpFill;
    public Text hpText;

    public Image mpFill;
    public Text mpText;

    public Image xpFill;
    public Text xpText;

    public Text levelText;

    public GameObject chargingUI;
    public Image chargingFill;

    public Image[] skillCD;
    public Text[] skillRequireMPText;
    private readonly Color[] requireMPColor = {new(0.286f, 0.5f, 0.6f), new(0.145f, 0.246f, 0.292f)};

    public Animator[] skillBind;
    private bool bindState;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroupSetting>();
    }

    public void UpdateHUD(PlayerController controller)
    {
        UpdateCanvasGroup();

        UpdateHPValue(controller.maxHP, controller.hp);
        UpdateMPValue(controller.maxMP, controller.mp);
        UpdateXPValue(controller.xp, controller.requireXP);
        UpdateLevel(controller.level);
        UpdateSkill(controller.skillCD, controller.skillLeftTime, controller.mp, controller.skillRequireMP);
        UpdateSkillBind(controller.skillBind);
        UpdateCharging(controller.playerAttack.isCharging, controller.playerAttack.chargingStrength);
    }

    private void UpdateCanvasGroup()
    {
        if (GamePlayManager.instance.isCutScene != cutSceneState)
        {
            cutSceneState = GamePlayManager.instance.isCutScene;

            if(cutSceneState)
                canvasGroup.Alpha1to0();
            else
                canvasGroup.Alpha0to1();
        }
    }

    private void UpdateHPValue(int maxHP, int hp)
    {
        float value = hp / (float)maxHP;
        hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount, value, Time.deltaTime * 20f);
        hpText.text = $"{hp}/{maxHP}";
    }

    private void UpdateMPValue(int maxMP, int mp)
    {
        float value = mp / (float)maxMP;
        mpFill.fillAmount = Mathf.Lerp(mpFill.fillAmount, value, Time.deltaTime * 20f);
        mpText.text = $"{mp}/{maxMP}";
    }

    private void UpdateXPValue(float xp, int requireXP)
    {
        xpFill.fillAmount = xp / requireXP;
        xpText.text = $"XP:{(int)xp}/{requireXP}";
    }

    private void UpdateLevel(int level)
    {
        levelText.text = $"Lv.{level}";
    }

    private void UpdateSkill(float[] cd, float[] leftTime, int mp, int[] skillRequireMP)
    {
        for(int i=0; i<cd.Length; i++)
        {
            skillCD[i].fillAmount = 1 - leftTime[i];
            skillRequireMPText[i].color = requireMPColor[skillRequireMP[i] <= mp ? 0 : 1];

        }
    }

    private void UpdateSkillBind(bool isBind)
    {
        if(bindState != isBind)
        {
            if (isBind)
                foreach (Animator anim in skillBind)
                    anim.SetTrigger("Bind");
            else
                foreach (Animator anim in skillBind)
                    anim.SetTrigger("UnBind");

            bindState = isBind;
        }
    }

    private void UpdateCharging(bool isCharging, float strength)
    {
        chargingUI.SetActive(isCharging);
        chargingFill.fillAmount = strength;
    }

    public void AlertMessage(string msg)
    {
        alertText.text = msg;
        alertTextCanvas.Alpha0to1(1, () => alertTextCanvas.Alpha1to0(2));
    }
}
