using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stage1Sequence : MonoBehaviour
{
    public SequenceExecuter startSequence;
    public SequenceExecuter bossSequence;
    public SequenceExecuter clearSequence;

    public List<UnityAction> startSequenceEnd = new();

    private void Start()
    {
        GamePlayManager.instance.isCutScene = true;

        startSequenceEnd.Add(() => GamePlayManager.instance.isCutScene = false);

        startSequence.StartSequence(() =>
        {
            foreach(UnityAction events in startSequenceEnd)
            {
                events?.Invoke();
            }
        });
    }

    public void BossSequence()
    {
        GamePlayManager.instance.isCutScene = true;
        bossSequence.StartSequence(() => GamePlayManager.instance.isCutScene = false);
    }

    public void ClearSequence()
    {
        GamePlayManager.instance.isCutScene = true;
        clearSequence.StartSequence();
    }
}
