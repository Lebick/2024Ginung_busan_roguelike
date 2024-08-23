using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillLockEnemy : EnemyController
{
    private Stage3Boss boss;

    private void Start()
    {
        boss = transform.parent.GetComponent<Stage3Boss>();
        player = boss.player;
        isFindPlayer = true;
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

    protected override void FollowPlayer()
    {
        if (boss.isTransformation) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 localDirection = transform.InverseTransformDirection(direction).normalized;
        sr.flipX = localDirection.x == 0 ? sr.flipX : localDirection.x < 0;
        rigid.AddForce(localDirection * moveSpeed);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (player.isDashing)
            {
                GetDamage(player.playerAttack.attackDamage, player.transform.position, player.knockbackStrength * 1.5f);
                player.mp += player.dashReturnMPValue;
                TimeScale.instance.SetTimeScale(0.1f, 0.2f);
            }
            else
            {
                player.GetDamage(contactDamage, transform.position, knockbackStrength);
                GamePlayManager.instance.player.SetSkillBind();
                SetTransformation();
            }
        }
    }

    protected override void OnDeath()
    {
        if (!boss.isTransformation)
            SetTransformation();
    }

    private void SetTransformation()
    {
        rigid.velocity = Vector2.zero;
        boss.SetTransformation();
    }
}
