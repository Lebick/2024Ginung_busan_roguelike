using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : Singleton<TimeScale>
{
    private IEnumerator coroutine;
    
    private void Start()
    {
        coroutine = ReturnTimeScale(0);
    }

    public void SetTimeScale(float value, float time)
    {
        Time.timeScale = value;
        StopCoroutine(coroutine);
        coroutine = ReturnTimeScale(time);
        StartCoroutine(coroutine);
    }

    private IEnumerator ReturnTimeScale(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
}
