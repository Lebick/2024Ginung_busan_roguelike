using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFade : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
    }

    public void ScaleTo0()
    {
        StartCoroutine(ChangeLifetime());
    }

    private IEnumerator ChangeLifetime()
    {
        float progress = 0f;
        while(progress <= 1f)
        {
            progress += Time.deltaTime / 5f;
            main.startLifetime = 3 * (1 - progress);
            yield return null;
        }
        yield return null;
    }
}
