using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefaultEnemy : EnemyController
{
    public float spawnRange = 3f;
    public GameObject defaultEnemy;

    private Stage3Boss boss;

    private void Start()
    {
        boss = transform.parent.GetComponent<Stage3Boss>();
        player = boss.player;
        isFindPlayer = true;

        for (int i=0; i<5f; i++)
        {
            Vector3 pos = transform.position + (Vector3)Random.insideUnitCircle * spawnRange;
            Instantiate(defaultEnemy, pos, Quaternion.Euler(0, 0, GamePlayManager.instance.currentRotation), GamePlayManager.instance.enemyParent);
        }
    }
    protected override void Update()
    {
        if (boss.isTransformation) return;
        base.Update();
    }

    public override void GetDamage(int damage, Vector3 hitObjectPos, float knockback)
    {
        if (boss.isTransformation) return;
        base.GetDamage(damage, hitObjectPos, knockback);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    protected override void OnDeath()
    {
        if (!boss.isTransformation)
            boss.SetTransformation();
    }
}
