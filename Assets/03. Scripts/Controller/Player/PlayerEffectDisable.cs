using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectDisable : MonoBehaviour
{
    public void OnEnd()
    {
        gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
    }
}
