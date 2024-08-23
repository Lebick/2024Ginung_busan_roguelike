using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeSend : MonoBehaviour
{
    public void SendShake(CameraShakeHandler handler, float detectRange, float findRange, float strength, float time)
    {
        float fixRange = (detectRange - findRange) / detectRange;

        handler.SetShake(fixRange, strength, time);
    }
}
