using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireBall : MonoBehaviour
{
    public float moveSpeed;

    public float knockbackStrength;
    public int damage;

    public GameObject spawnEffect;

    private void Start()
    {
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        if (!IsCameraIn(transform.position)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyController>() != null)
            collision.GetComponent<EnemyController>().GetDamage(damage, transform.position, knockbackStrength);
    }

    private bool IsCameraIn(Vector3 pos)
    {
        Vector3 fixPos = Camera.main.WorldToViewportPoint(pos);
        Vector3 clampPos = new(Mathf.Clamp(fixPos.x, -0.2f, 1.2f), Mathf.Clamp(fixPos.y, -0.2f, 1.2f), fixPos.z);

        if (fixPos != clampPos) return false;

        return true;
    }
}
