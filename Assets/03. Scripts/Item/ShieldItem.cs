using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.GetInvincibleState();

        GameHUD.instance.SpawnAlertMessage("���� ������ ȹ��!\n5�ʰ� ����", Color.white);
    }
}
