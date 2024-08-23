using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public Transform canvas;

    public GameObject enemySelect;
    public GameObject itemSelect;

    private GameObject enemySelectInstantiate;
    private GameObject itemSelectInstantiate;

    private Color cheatAlertBG = new(1, 0.8f, 0.66f);

    private void Update()
    {
        for(int i=(int)KeyCode.F1; i<=(int)KeyCode.F7; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                Invoke($"Cheat{i-(int)KeyCode.F1}",0);
            }
        }
    }

    private void Cheat0()
    {
        int stage = SceneLoadManager.instance.currentScene + 1;
        stage = stage <= 3 ? stage : 1;
        SceneLoadManager.instance.SceneChange(stage);
    }

    private void Cheat1()
    {
        if (GamePlayManager.instance.isPause)
        {
            GameHUD.instance.SpawnAlertMessage("현재 해당 치트키를\n사용할 수 없습니다.", cheatAlertBG);
            return;
        }

        GameHUD.instance.SpawnAlertMessage("레벨 상승\n치트키 사용", cheatAlertBG);
        GamePlayManager.instance.player.LevelUP();
    }

    private void Cheat2()
    {
        GameHUD.instance.SpawnAlertMessage($"플레이어 무적\n치트키 {(GamePlayManager.instance.player.isCheatInvincibility ? "비활성화" : "활성화")}", cheatAlertBG);
        GamePlayManager.instance.player.SetInvincibleState();
    }

    private void Cheat3()
    {
        GameHUD.instance.SpawnAlertMessage("체력&마나 회복\n치트키 사용", cheatAlertBG);
        GamePlayManager.instance.player.hp = GamePlayManager.instance.player.maxHP;
        GamePlayManager.instance.player.mp = GamePlayManager.instance.player.maxMP;
    }

    private void Cheat4()
    {
        if (enemySelectInstantiate == null)
        {
            enemySelectInstantiate = Instantiate(enemySelect, canvas);
            GameHUD.instance.SpawnAlertMessage("적 선택 등장\n치트키 활성화", cheatAlertBG);
        }
        else
        {
            Destroy(enemySelectInstantiate);
            GameHUD.instance.SpawnAlertMessage("적 선택 등장\n치트키 비활성화", cheatAlertBG);
        }
            
    }

    private void Cheat5()
    {
        GameHUD.instance.SpawnAlertMessage("아이템 선택 사용\n치트키 사용", cheatAlertBG);
        print("미구현");

        if (itemSelectInstantiate == null)
            itemSelectInstantiate = Instantiate(itemSelect, canvas);
        else
            Destroy(itemSelectInstantiate);
    }

    private void Cheat6()
    {
        GameHUD.instance.SpawnAlertMessage("모든 적 처치\n치트키 사용", cheatAlertBG);
        foreach (EnemyController enemy in GamePlayManager.instance.enemys)
        {
            enemy.ForceDeath();
        }
    }
}
