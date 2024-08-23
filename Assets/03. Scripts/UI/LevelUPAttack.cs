using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUPAttack : MonoBehaviour
{
    private PlayerController player;

    public LevelUP levelUP;

    private void Start()
    {
        player = GamePlayManager.instance.player;
    }
    public void _0_0_OnClickAttackTypeSelect(int index)
    {
        player.attackType = index;

        if (index == 0)
            player.playerAttack.attackDamage *= 2;

        if (index == 1)
            player.isTPtoDash = true;

        levelUP.SelectAttack();
    }

    public void _0_1_OnClickDamageUP()
    {
        player.playerAttack.attackDamage = (int)Mathf.Round(player.playerAttack.attackDamage * 1.5f);
        levelUP.SelectAttack();
    }

    public void _1_0_OnClickAttackRange()
    {
        player.playerAttack.normalAttackRange += 0.2f;
        levelUP.SelectAttack();
    }

    public void _2_0_OnClickDashRange()
    {
        player.dashDistance += 3f;
        levelUP.SelectAttack();
    }

    public void _2_1_OnClickMPReturn()
    {
        if (player.dashReturnMPValue < 50)
            player.dashReturnMPValue = 50;
        else
            player.dashReturnMPValue += 10;
        levelUP.SelectAttack();
    }

    public void _2_2_OnClickMPRequireDown()
    {
        player.skillRequireMP[1] = Mathf.Max(0, player.skillRequireMP[1] - 5);
        levelUP.SelectAttack();
    }

    public void _3_0_OnClickChargingDown()
    {
        player.playerAttack.maxChargingTime *= 0.8f;
        levelUP.SelectAttack();
    }

    public void _3_1_OnClickEnemyThrough()
    {
        player.playerAttack.maxArrowThrough++;
        levelUP.SelectAttack();
    }

    public void _4_0_OnClickSummonCreature()
    {
        player.playerAttack.SummonMinion();
        levelUP.SelectAttack();
    }

    public void _4_1_OnClickGetShield()
    {
        player.summonerShieldCD = Mathf.Max(0, player.summonerShieldCD-5);
        player.SetSummonerShield();
        levelUP.SelectAttack();
    }

    public void _5_OnClickSkillButton(int index)
    {
        switch (index)
        {
            case 0:
                GamePlayManager.instance.player.skillRequireMP[0] = Mathf.Max(0, GamePlayManager.instance.player.skillRequireMP[0] - 5);
                break;

            case 1:
                GamePlayManager.instance.player.teleportRange += 0.5f;
                break;

            case 2:
                GamePlayManager.instance.player.playerAttack.attackSpeedBuff += 0.1f;
                break;

            case 3:
                GamePlayManager.instance.player.hpRecoverySkillValue += 5;
                break;
        }
        levelUP.SelectSkill();
    }
}
