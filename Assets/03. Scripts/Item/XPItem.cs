using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.GetXP(50);
        GameHUD.instance.SpawnAlertMessage("����ġ ������ ȹ��!\n��� ����ġ 50 ȹ��", Color.white);
    }
}
