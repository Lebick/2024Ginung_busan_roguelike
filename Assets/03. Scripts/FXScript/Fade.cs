using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject fadeImage;
    public Material fadeMaterial;

    public Texture[] fadeType;

    private void Start()
    {
        fadeMaterial.SetFloat("_Progress", 0);
    }

    public void Fade0to1()
    {
        fadeMaterial.SetTexture("_ImageTex", fadeType[0]);
        fadeImage.SetActive(true);
        StartCoroutine(Fading(0, 1));
    }

    public void Fade1to0()
    {
        fadeMaterial.SetTexture("_ImageTex", fadeType[1]);
        fadeImage.SetActive(true);
        StartCoroutine(Fading(1, 0));
    }

    private IEnumerator Fading(float start, float end)
    {
        float progress = 0f;
        while(progress < 1f)
        {
            progress += Time.deltaTime;
            fadeMaterial.SetFloat("_Progress", Mathf.Lerp(start, end, progress));
            yield return null;
        }

        fadeImage.SetActive(end > 0);
    }
}
