using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupSetting : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private IEnumerator coroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        coroutine = SetAlpha(1, 0, 0);
    }

    public void Alpha0to1(float time = 1, Action OnEnd = null, bool isUnScaledTime = false)
    {
        StopCoroutine(coroutine);
        coroutine = SetAlpha(time, 0, 1, OnEnd, isUnScaledTime);
        StartCoroutine(coroutine);
    }
    
    public void Alpha1to0(float time = 1,Action OnEnd = null, bool isUnScaledTime = false)
    {
        StopCoroutine(coroutine);
        coroutine = SetAlpha(time, 1, 0, OnEnd, isUnScaledTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator SetAlpha(float time, float start, float end, Action OnEnd = null, bool isUnScaledTime = false)
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += (isUnScaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
            canvasGroup.alpha = Mathf.Lerp(start, end, progress);
            yield return null;
        }

        OnEnd?.Invoke();
    }
}
