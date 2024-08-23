using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stage1to2Sequence : MonoBehaviour
{
    public SequenceExecuter startSequence;

    private void Start()
    {
        GamePlayManager.instance.isCutScene = true;

        startSequence.StartSequence(() =>
        {
            SceneLoadManager.instance.SceneChange(SceneLoadManager.instance.currentScene+1);
        });
    }
}
