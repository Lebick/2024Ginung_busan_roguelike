using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.isEnemyStop = true;
        GameHUD.instance.SpawnAlertMessage("���� ������ ȹ��!\n10�ʰ� ��� �� ����", Color.white);

        Invoke(nameof(ReturnItem), 10f);
    }

    private void ReturnItem()
    {
        GamePlayManager.instance.isEnemyStop = false;
    }
}
