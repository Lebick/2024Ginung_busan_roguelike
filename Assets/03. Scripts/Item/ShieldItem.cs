using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.GetInvincibleState();

        GameHUD.instance.SpawnAlertMessage("무적 아이템 획득!\n5초간 무적", Color.white);
    }
}
