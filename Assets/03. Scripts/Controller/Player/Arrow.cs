using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public float knockback;

    public float speed = 10f;

    public int maxThroughEnemy;
    private int throughEnemyCount;

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemy.GetDamage(damage, transform.position, knockback);

            if(++throughEnemyCount >= maxThroughEnemy)
                Destroy(gameObject);
        }
    }
}
