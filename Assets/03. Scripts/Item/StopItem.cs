using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.isEnemyStop = true;
        GameHUD.instance.SpawnAlertMessage("정지 아이템 획득!\n10초간 모든 적 정지", Color.white);

        Invoke(nameof(ReturnItem), 10f);
    }

    private void ReturnItem()
    {
        GamePlayManager.instance.isEnemyStop = false;
    }
}
