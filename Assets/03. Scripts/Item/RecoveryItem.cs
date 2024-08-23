using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.hp += 50;
        GamePlayManager.instance.player.mp += 50;

        GameHUD.instance.SpawnAlertMessage("ȸ�� ������ ȹ��!\nü��&���� 50 ȸ��", Color.white);
    }
}
