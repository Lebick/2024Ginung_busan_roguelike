using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCanvas : MonoBehaviour
{
    private CanvasGroupSetting canvasGroup;
    private bool cutSceneState;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroupSetting>();
    }

    private void Update()
    {
        UpdateCanvasGroup();
    }

    private void UpdateCanvasGroup()
    {
        if (GamePlayManager.instance.isCutScene != cutSceneState)
        {
            cutSceneState = GamePlayManager.instance.isCutScene;

            if (cutSceneState)
                canvasGroup.Alpha1to0();
            else
                canvasGroup.Alpha0to1();
        }
    }
}
