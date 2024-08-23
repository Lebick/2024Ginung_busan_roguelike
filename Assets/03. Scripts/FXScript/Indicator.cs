using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject indicator;
    private RectTransform indicatorRect;
    private RectTransform canvas;
    public float edgePadding = 100f;
    public float angleOffset;

    private void Start()
    {
        canvas = GamePlayManager.instance.indicatorCanvas;
    }

    private void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isOffScreen = screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;

        if (isOffScreen)
        {
            if (indicatorRect == null)
                indicatorRect = Instantiate(indicator, transform.position, Quaternion.identity, canvas).GetComponent<RectTransform>();

            //뷰포인트 위치값을 0~1값으로 제한한다. (화면 밖이라는 가정하에 작동되므로 0혹은 1이 반환된다.)
            Vector2 viewportPos = new Vector2(Mathf.Clamp01(screenPoint.x), Mathf.Clamp01(screenPoint.y));

            //뷰포인트 좌표를 스크린좌표로 변경한 후, 캔버스좌표로 변환한다.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Camera.main.ViewportToScreenPoint(viewportPos), null, out Vector2 canvasPos);

            //변환한 위치로 이동한다.
            indicatorRect.anchoredPosition = canvasPos;

            //사전에 정한 패딩값만큼의 여유를 준다.
            indicatorRect.anchoredPosition = new Vector2(
                Mathf.Clamp(indicatorRect.anchoredPosition.x, -canvas.rect.width / 2 + edgePadding, canvas.rect.width / 2 - edgePadding),
                Mathf.Clamp(indicatorRect.anchoredPosition.y, -canvas.rect.height / 2 + edgePadding, canvas.rect.height / 2 - edgePadding));

            Vector2 direction = canvasPos.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            indicatorRect.rotation = Quaternion.Euler(0, 0, angle);

        }
        else if (indicatorRect != null) Destroy(indicatorRect.gameObject);
    }

    private void OnDestroy()
    {
        if (indicatorRect != null)
            Destroy(indicatorRect.gameObject);
    }
}
