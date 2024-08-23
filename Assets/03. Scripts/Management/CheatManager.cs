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
            GameHUD.instance.SpawnAlertMessage("���� �ش� ġƮŰ��\n����� �� �����ϴ�.", cheatAlertBG);
            return;
        }

        GameHUD.instance.SpawnAlertMessage("���� ���\nġƮŰ ���", cheatAlertBG);
        GamePlayManager.instance.player.LevelUP();
    }

    private void Cheat2()
    {
        GameHUD.instance.SpawnAlertMessage($"�÷��̾� ����\nġƮŰ {(GamePlayManager.instance.player.isCheatInvincibility ? "��Ȱ��ȭ" : "Ȱ��ȭ")}", cheatAlertBG);
        GamePlayManager.instance.player.SetInvincibleState();
    }

    private void Cheat3()
    {
        GameHUD.instance.SpawnAlertMessage("ü��&���� ȸ��\nġƮŰ ���", cheatAlertBG);
        GamePlayManager.instance.player.hp = GamePlayManager.instance.player.maxHP;
        GamePlayManager.instance.player.mp = GamePlayManager.instance.player.maxMP;
    }

    private void Cheat4()
    {
        if (enemySelectInstantiate == null)
        {
            enemySelectInstantiate = Instantiate(enemySelect, canvas);
            GameHUD.instance.SpawnAlertMessage("�� ���� ����\nġƮŰ Ȱ��ȭ", cheatAlertBG);
        }
        else
        {
            Destroy(enemySelectInstantiate);
            GameHUD.instance.SpawnAlertMessage("�� ���� ����\nġƮŰ ��Ȱ��ȭ", cheatAlertBG);
        }
            
    }

    private void Cheat5()
    {
        GameHUD.instance.SpawnAlertMessage("������ ���� ���\nġƮŰ ���", cheatAlertBG);
        print("�̱���");

        if (itemSelectInstantiate == null)
            itemSelectInstantiate = Instantiate(itemSelect, canvas);
        else
            Destroy(itemSelectInstantiate);
    }

    private void Cheat6()
    {
        GameHUD.instance.SpawnAlertMessage("��� �� óġ\nġƮŰ ���", cheatAlertBG);
        foreach (EnemyController enemy in GamePlayManager.instance.enemys)
        {
            enemy.ForceDeath();
        }
    }
}
