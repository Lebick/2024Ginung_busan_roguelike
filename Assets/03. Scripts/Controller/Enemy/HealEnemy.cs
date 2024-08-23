using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealEnemy : EnemyController
{
    public List<Transform> nearEnemys = new();
    private List<GameObject> wasHealEnemy = new();

    public float healRange;
    public int healValue;

    public GameObject healEffect;

    protected override void Update()
    {
        FindNearEnemys();

        EnemyHeal();

        base.Update();
    }

    private void FindNearEnemys()
    {
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, healRange, LayerMask.GetMask("Enemy"));
        nearEnemys = new();

        foreach(Collider2D enemy in enemys)
        {
            if (wasHealEnemy.Contains(enemy.gameObject) || enemy.gameObject == gameObject) continue;

            nearEnemys.Add(enemy.transform);
        }

        nearEnemys = nearEnemys.OrderBy(a => Vector3.Distance(transform.position, a.position)).ToList();
    }

    private void EnemyHeal()
    {
        if (nearEnemys.Count <= 0) return;

        Transform target = nearEnemys[0];

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 localDirection = transform.InverseTransformDirection(direction).normalized;
        sr.flipX = localDirection.x == 0 ? sr.flipX : localDirection.x < 0;
        transform.Translate(localDirection * moveSpeed * Time.deltaTime);
    }

    protected override void FollowPlayer()
    {
        if (nearEnemys.Count > 0) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 localDirection = transform.InverseTransformDirection(direction).normalized;
        sr.flipX = localDirection.x == 0 ? sr.flipX : localDirection.x < 0;
        transform.Translate(-localDirection * moveSpeed * Time.deltaTime); //¹Ý´ë·Î µµ¸Á
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (nearEnemys.Contains(collision.transform))
        {
            wasHealEnemy.Add(collision.gameObject);
            collision.gameObject.GetComponent<EnemyController>().hp += 5;

            Instantiate(healEffect, transform.position, Quaternion.identity);
        }

        base.OnCollisionEnter2D(collision);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
