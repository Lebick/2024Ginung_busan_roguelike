using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private void Update()
    {
        if(Cursor.visible)
            Cursor.visible = false;

        FollowMouse();
    }

    private void FollowMouse()
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = 0;
        //transform.position = mousePos;

        Vector2 mouse = Input.mousePosition;
        transform.position = mouse;
    }
}
