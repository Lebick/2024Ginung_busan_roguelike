using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeHandler : MonoBehaviour
{
    public void SetShake(float dis, float strength, float time)
    {
        CameraShakeManager.instance.StartShake(strength * dis, time);
    }
}
