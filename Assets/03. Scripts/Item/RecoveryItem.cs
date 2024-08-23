using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.hp += 50;
        GamePlayManager.instance.player.mp += 50;

        GameHUD.instance.SpawnAlertMessage("회복 아이템 획득!\n체력&마나 50 회복", Color.white);
    }
}
