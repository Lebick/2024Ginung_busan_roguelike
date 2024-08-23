using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : Singleton<Blind>
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetBlind()
    {
        if (IsInvoking(nameof(StopBlind)))
        {
            CancelInvoke(nameof(StopBlind));
        }
        else
        {
            anim.SetTrigger("Start");
        }

        Invoke(nameof(StopBlind), 5f);
    }

    private void StopBlind()
    {
        anim.SetTrigger("Stop");
    }
}
