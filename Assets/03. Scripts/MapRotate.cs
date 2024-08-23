using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

public class MapRotate : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public float rotateSpeed = 10f;
    public bool isReverse;

    private List<Vector3> pos;
    public List<float> angle;
    private Transform player;

    public float maxRotate = 90;

    private float currentRotation;
    private float newRotation;
    private void Start()
    {
        player = GamePlayManager.instance.player.transform;

        Spline spline = spriteShapeController.spline;
        int count = spline.GetPointCount();
        pos = new();
        angle = new();

        for (int i = 0; i < count; i++)
        {
            pos.Add(spline.GetPosition(i));

            angle.Add((isReverse ? 360 : 0) - (maxRotate / (count - 1)) * i);
        }
    }

    private void Update()
    {
        FindNearPoint();
        RotateObj();
    }

    private void FindNearPoint()
    {
        float nearestDistance = Mathf.Infinity;

        for (int i = 0; i < pos.Count; i++)
        {
            float distance = Vector3.Distance(player.position, pos[i]);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                newRotation = angle[i];
            }
        }
    }

    private void RotateObj()
    {
        Vector3 rot = new(0, 0, Mathf.LerpAngle(currentRotation, newRotation, Time.deltaTime * rotateSpeed));
        currentRotation = rot.z;

        player.localEulerAngles = rot;
        foreach (EnemyController enemy in GamePlayManager.instance.enemys)
        {
            if (enemy == null) continue;

            Transform tr = enemy.transform;
            tr.localEulerAngles = rot;
        }

        foreach (Transform obj in GamePlayManager.instance.rotationObj.GetComponentInChildren<Transform>())
        {
            if (obj == null || obj == GamePlayManager.instance.rotationObj) continue;

            obj.localEulerAngles = rot;
        }

        GamePlayManager.instance.currentRotation = currentRotation;
    }

}
