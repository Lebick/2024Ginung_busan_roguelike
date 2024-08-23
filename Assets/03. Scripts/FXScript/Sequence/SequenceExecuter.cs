using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceExecuter : MonoBehaviour
{
    public List<Sequence> sequences;

    private int endCount;

    public void StartSequence(Action OnEnd = null)
    {
        foreach(Sequence sequence in sequences)
        {
            sequence.StartSequence(() =>
            {
                if(IsEnd())
                    OnEnd?.Invoke();
            });
        }
    }

    private bool IsEnd()
    {
        return ++endCount >= sequences.Count;
    }
}
