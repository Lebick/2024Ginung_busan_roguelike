using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    private float currentStrength;

    private IEnumerator shakeCoroutine;

    public void SequenceShake_05sec(float strength)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        shakeCoroutine = Shake(strength, 0.5f);
        StartCoroutine(shakeCoroutine);
    }

    public void SequenceShake_30sec(float strength)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        shakeCoroutine = Shake(strength, 3.0f);
        StartCoroutine(shakeCoroutine);
    }

    public void StartShake(float strength, float time)
    {
        if(strength >= currentStrength)
        {
            currentStrength = strength;
            
            if(shakeCoroutine != null) StopCoroutine(shakeCoroutine);

            shakeCoroutine = Shake(strength, time);
            StartCoroutine(shakeCoroutine);
        }
    }

    private IEnumerator Shake(float strength, float time)
    {
        float progress = 0;

        while (progress <= 1f)
        {
            transform.localPosition = Random.insideUnitCircle * strength;
            progress += Time.deltaTime / time;
            yield return null;
        }

        transform.localPosition = Vector3.zero;

        currentStrength = 0;
    }
}
