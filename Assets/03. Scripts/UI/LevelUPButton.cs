using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUPButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerController player;

    [TextArea]
    public string myDescription = string.Empty;

    public LevelUP levelUP;

    public void OnPointerEnter(PointerEventData eventData)
    {
        levelUP.alertMessage.text = myDescription;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        levelUP.alertMessage.text = string.Empty;
    }
}
