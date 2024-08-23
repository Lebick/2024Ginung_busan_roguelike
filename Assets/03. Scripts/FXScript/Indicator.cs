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

            //������Ʈ ��ġ���� 0~1������ �����Ѵ�. (ȭ�� ���̶�� �����Ͽ� �۵��ǹǷ� 0Ȥ�� 1�� ��ȯ�ȴ�.)
            Vector2 viewportPos = new Vector2(Mathf.Clamp01(screenPoint.x), Mathf.Clamp01(screenPoint.y));

            //������Ʈ ��ǥ�� ��ũ����ǥ�� ������ ��, ĵ������ǥ�� ��ȯ�Ѵ�.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Camera.main.ViewportToScreenPoint(viewportPos), null, out Vector2 canvasPos);

            //��ȯ�� ��ġ�� �̵��Ѵ�.
            indicatorRect.anchoredPosition = canvasPos;

            //������ ���� �е�����ŭ�� ������ �ش�.
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
