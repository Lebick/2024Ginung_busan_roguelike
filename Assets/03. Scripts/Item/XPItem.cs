using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPItem : Item
{
    public override void UseItem()
    {
        GamePlayManager.instance.player.GetXP(50);
        GameHUD.instance.SpawnAlertMessage("∞Ê«Ëƒ° æ∆¿Ã≈€ »πµÊ!\n¡ÔΩ√ ∞Ê«Ëƒ° 50 »πµÊ", Color.white);
    }
}
