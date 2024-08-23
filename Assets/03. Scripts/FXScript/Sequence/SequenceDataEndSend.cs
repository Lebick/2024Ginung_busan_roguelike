using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceDataEndSend : MonoBehaviour
{
    public float timer;

    public void EndSend(Action OnEnd)
    {
        StartCoroutine(Wait(OnEnd));
    }

    private IEnumerator Wait(Action OnEnd)
    {
        yield return new WaitForSeconds(timer);
        OnEnd?.Invoke();
    }
}
