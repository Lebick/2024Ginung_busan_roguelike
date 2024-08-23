using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    public List<SequenceData> sequenceDatas;

    private int currentIndex;

    private Action OnEnd;

    public void StartSequence(Action OnEnd)
    {
        this.OnEnd = OnEnd;

        PlaySequence(currentIndex);
    }

    private void PlaySequence(int index)
    {
        sequenceDatas[index].PlaySequence(() =>
        {
            if (IsEnd())
                OnEnd?.Invoke();
            else
                PlaySequence(currentIndex);
        });
    }

    private bool IsEnd()
    {
        return ++currentIndex >= sequenceDatas.Count;
    }
}
