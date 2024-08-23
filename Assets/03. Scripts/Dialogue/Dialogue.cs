using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    private CanvasGroupSetting canvasGroup;

    public Text text;
    public string msg;
    public float waitTime = 0.2f;

    public Vector2 offset;

    public void Setting(string msg)
    {
        this.msg = msg;
    }

    private void Start()
    {
        transform.SetParent(GamePlayManager.instance.overlayCanvas, false);

        canvasGroup = GetComponent<CanvasGroupSetting>();

        GetComponent<RectTransform>().anchoredPosition += offset;
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        string currentMsg = string.Empty;
        bool isColor = false;

        for (int i=0; i<msg.Length; i++)
        {
            switch (msg[i])
            {
                case 'a': //빨간색
                    currentMsg += "<color=#ff6666>";
                    isColor = true;
                    continue;

                case 's': //초록색
                    currentMsg += "<color=#66aaff>";
                    isColor = true;
                    continue;

                case 'd': //파란색
                    currentMsg += "<color=#99ff99>";
                    isColor = true;
                    continue;

                case 'f': //검은색
                    currentMsg += "</color>";
                    isColor = false;
                    continue;
            }
            currentMsg += msg[i];

            text.text = currentMsg + (isColor ? "</color>" : "");

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(0.5f);
        canvasGroup.Alpha1to0(0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
