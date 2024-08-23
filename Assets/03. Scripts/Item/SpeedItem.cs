using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.moveSpeedBuff += 0.2f;
        GameHUD.instance.SpawnAlertMessage("�̵��ӵ� ����!\n������ 0.2����", Color.white);
    }
}
