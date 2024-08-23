using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelUP : MonoBehaviour
{
    private Animator anim;

    private CanvasGroup canvasGroup;
    private CanvasGroupSetting canvasGroupSetting;

    public Text alertMessage;

    public GameObject[] selectAttacks;

    public Item[] items;

    public void Start()
    {
        transform.localPosition = Vector3.zero;
        anim = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroupSetting = GetComponent<CanvasGroupSetting>();

        int level = GamePlayManager.instance.player.level;
        int index = GamePlayManager.instance.player.attackType;

        if (level <= 2)
            ActiveSelect(selectAttacks[0]);
        else
            ActiveSelect(selectAttacks[index + 1]);
    }


    private void ActiveSelect(GameObject select)
    {
        foreach(GameObject obj in selectAttacks)
            obj.SetActive(false);

        select.SetActive(true);
    }

    public void SelectAttack()
    {
        anim.SetTrigger("AttackSelected");
        canvasGroup.interactable = false;
    }

    public void EndAnim()
    {
        canvasGroup.interactable = true;
    }

    public void SelectSkill()
    {
        canvasGroup.interactable = false;
        canvasGroupSetting.Alpha1to0(1, () =>
        {
            Time.timeScale = 1f;
            GamePlayManager.instance.isPause = false;
            GetItem();
            Destroy(gameObject);
        }, true);
    }

    private void GetItem()
    {
        int index = Random.Range(0, items.Length);
        items[index].UseItem();
    }
}
