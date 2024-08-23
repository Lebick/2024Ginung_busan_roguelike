using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.moveSpeedBuff += 0.2f;
        GameHUD.instance.SpawnAlertMessage("이동속도 증가!\n영구적 0.2증가", Color.white);
    }
}
