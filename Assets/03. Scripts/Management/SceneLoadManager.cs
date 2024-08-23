using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNames
{
    Title = 0,
    Stage1 = 1,
    Move2 = 2,
    Stage2 = 3,
    Move3 = 4,
    Stage3 = 5,
    Ranking = 6
}

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    public Fade fadeImage;

    public int currentScene;

    private bool isLoading;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void SceneChange(int scene)
    {
        if (isLoading) return;

        isLoading = true;

        fadeImage.Fade0to1();
        StartCoroutine(SceneLoad(scene));
    }

    public void SceneChange(SceneNames scene)
    {
        fadeImage.Fade0to1();
        StartCoroutine(SceneLoad((int)scene));
    }

    private IEnumerator SceneLoad(int scene)
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation sc = SceneManager.LoadSceneAsync(scene);
        sc.allowSceneActivation = false;

        while (sc.progress < 0.9f)
            yield return null;

        yield return new WaitForSeconds(0.5f);
        sc.allowSceneActivation = true;

        fadeImage.Fade1to0();

        yield return new WaitForSeconds(1f);
        isLoading = false;
    }
}
