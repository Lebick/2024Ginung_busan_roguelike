using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossHealEnemy : EnemyController
{
    public float healTime = 10f;

    private Stage3Boss boss;

    private bool isHealing;

    private void Start()
    {
        boss = transform.parent.GetComponent<Stage3Boss>();
        player = boss.player;
        isFindPlayer = true;
        isHealing = true;

        Vector3 targetPos = EnemySpawnManager.instance.enemySpawnPos.OrderBy(a => (Vector3.Distance(player.transform.position, a.position))).Last().position;
        transform.position = targetPos;
        Invoke(nameof(EndHeal), healTime);
    }

    private void EndHeal()
    {
        isHealing = false;
        boss.GetHeal();
        SetTransformation();
    }

    protected override void Update()
    {
        if (isHealing)
        {
            boss.recoveryHP += Time.deltaTime * 3f;
        }
    }

    public override void GetDamage(int damage, Vector3 hitObjectPos, float knockback)
    {
        if (boss.isTransformation) return;

        if (isHealing)
        {
            CancelInvoke(nameof(EndHeal));
            Invoke(nameof(Transformation), 5f);
            isHealing = false;
            boss.recoveryHP = 0;
        }

        base.GetDamage(damage, hitObjectPos, knockback);
    }

    private void Transformation()
    {
        SetTransformation();
    }

    protected override void OnDeath()
    {
        SetTransformation();
    }

    private void SetTransformation()
    {
        if (boss.isTransformation) return;
        boss.SetTransformation();
    }
}
