using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogue;
    private Transform canvas;

    private void Start()
    {
        canvas = GamePlayManager.instance.overlayCanvas;
    }

    public void SpawnDialogue(string msg)
    {
        Dialogue newDialogue = Instantiate(dialogue, canvas);
        newDialogue.Setting(msg);
    }
}
