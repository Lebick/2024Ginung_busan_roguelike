using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SequenceDataEndSend))]
public class SequenceData : MonoBehaviour
{
    private SequenceDataEndSend sequenceDataEndSend;
    public List<UnityEvent> datas = new();

    private void Awake()
    {
        sequenceDataEndSend = GetComponent<SequenceDataEndSend>();
    }

    public void PlaySequence(Action OnEnd)
    {
        foreach(UnityEvent data in datas)
            data?.Invoke();

        print(name);
        sequenceDataEndSend.EndSend(OnEnd);
    }
}
