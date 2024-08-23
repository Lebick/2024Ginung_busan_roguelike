using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneSkip : MonoBehaviour
{
    public Text message;

    private float escapePressTime;

    private bool isStop;

    private void Update()
    {
        if (GamePlayManager.instance.isCutScene)
        {
            isStop = false;
            CheckSkip();
        }
        else if(!isStop)
        {
            Time.timeScale = 1.0f;
            isStop = true;
            message.text = string.Empty;
            escapePressTime = 0;
        }
    }

    private void CheckSkip()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            escapePressTime += Time.deltaTime;
            message.text = "1�ʰ� ���� �� ��ӵ˴ϴ�.";
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            escapePressTime = 0;
            message.text = string.Empty;
        }

        if(escapePressTime > 1f)
        {
            Time.timeScale = 10.0f;
            message.text = "10��� �� �Դϴ�.";
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
