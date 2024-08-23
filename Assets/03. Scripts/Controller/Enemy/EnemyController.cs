using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Controller
{
    public float xp;

    public float detectRange;
    public float attackRange;
    public float moveSpeed;

    public int attackDamage;
    public int contactDamage;

    public PlayerController player;

    protected bool isFindPlayer;
    public GameObject playerFindEffect;

    public ExperienceOrb experienceOrb;

    private bool isAttack;

    private bool isForceDeath;

    public float attackCD;

    protected override void Update()
    {
        if (GamePlayManager.instance.isCutScene || GamePlayManager.instance.isPause) return;

        base.Update();

        if(player == null)
            FindPlayer();

        if (isFindPlayer)
        {
            FollowPlayer();
            AttackPlayer();
        }
    }

    private void FindPlayer()
    {
        Collider2D nearObject = Physics2D.OverlapCircle(transform.position, detectRange, LayerMask.GetMask("Player"));

        if(nearObject != null)
        {
            Invoke(nameof(FindDelay), 0.5f);
            player = nearObject.GetComponent<PlayerController>();
            Instantiate(playerFindEffect, transform.position, playerFindEffect.transform.rotation);
        }
    }

    private void FindDelay()
    {
        isFindPlayer = true;
    }

    protected virtual void FollowPlayer()
    {
        if (isAttack) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 localDirection = transform.InverseTransformDirection(direction).normalized;
        sr.flipX = localDirection.x == 0 ? sr.flipX : localDirection.x < 0;
        transform.Translate(localDirection * moveSpeed * Time.deltaTime);
    }

    protected virtual void AttackPlayer()
    {
        if(IsPlayerAttackRange() && !isAttack && isFindPlayer)
        {
            anim.SetTrigger("Attack");
            isAttack = true;
            Invoke(nameof(ReturnAttackState), attackCD);
        }
    }

    private bool IsPlayerAttackRange()
    {
        if (attackRange == 0) return false;

        Collider2D nearObject = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));

        return nearObject != null;
    }

    public override void GetDamage(int damage, Vector3 hitObjectPos, float knockback)
    {
        base.GetDamage(damage, hitObjectPos, knockback);

        if(player == null)
        {
            Invoke(nameof(FindDelay), 0.5f);
            player = GamePlayManager.instance.player;
            Instantiate(playerFindEffect, transform.position, transform.rotation);
        }
    }

    protected override void OnDeath()
    {
        if (!isForceDeath)
        {
            int xpCount = Random.Range(1, 4);
            for (int i = 0; i < xpCount; i++)
            {
                ExperienceOrb orb = Instantiate(experienceOrb, transform.position, Quaternion.identity, GamePlayManager.instance.InstantiateObjectParent);
                orb.xp = xp / xpCount;
            }
        }
        base.OnDeath();
    }

    public virtual void ForceDeath()
    {
        isForceDeath = true;
        base.GetDamage(1000, GamePlayManager.instance.player.transform.position, 5f);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (player.isDashing)
            {
                GetDamage(player.playerAttack.attackDamage, player.transform.position, player.knockbackStrength * 1.5f);
                player.mp += player.dashReturnMPValue;
                TimeScale.instance.SetTimeScale(0.1f, 0.2f);
            }
            else
            {
                player.GetDamage(contactDamage, transform.position, 3);
            }
        }
    }

    public virtual void Attack()
    {
        if (IsPlayerAttackRange())
            player.GetDamage(attackDamage, transform.position, knockbackStrength);
    }

    public void ReturnAttackState()
    {
        isAttack = false;
    }
}
