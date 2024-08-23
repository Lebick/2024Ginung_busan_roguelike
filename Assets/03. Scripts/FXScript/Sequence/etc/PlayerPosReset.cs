using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosReset : MonoBehaviour
{
    public Vector3 pos;

    public void PosReset()
    {
        transform.position = pos;
    }
}
