using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isCanAttack = true;

    public Transform attackAxis;

    public int attackDamage;
    public float attackCD;

    public float attackSpeedBuff = 1.5f;
    public float currentAttackSpeedBuff = 1.0f;
    public float attackBuffDuration = 4f;

    private float knockback;

    public float normalAttackRange = 1f;
    public Transform normalAttack;
    public GameObject normalAttackEffect;
    private Vector3 normalAttackScale;

    public GameObject arrow;
    public bool isCharging;
    public float maxChargingTime;
    private float chargingTimer;
    public float chargingStrength;
    public int maxArrowThrough; //최대 관통

    public GameObject minion;

    public int minionAmount;

    private void Start()
    {
        isCanAttack = true;
        normalAttackScale = normalAttack.transform.localScale;
    }

    public void Attack(int attackType, float knockback)
    {
        isCanAttack = false;
        this.knockback = knockback;

        switch (attackType)
        {
            case 0:
            case 1:
                NormalAttack(); break;

            case 2:
                BowAttack(); break;

            case 3: break;
        }
    }

    private void ReturnCanAttackState()
    {
        isCanAttack = true;
    }

    private void Update()
    {
        BowCharging();
    }

    private void NormalAttack()
    {
        normalAttack.localScale = normalAttackScale * normalAttackRange;

        Collider2D[] rangeEnemys = Physics2D.OverlapBoxAll(normalAttack.position, normalAttack.localScale, 0, LayerMask.GetMask("Enemy"));

        normalAttackEffect.SetActive(true);
        foreach (Collider2D enemy in rangeEnemys)
        {
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();
            enemyScript.GetDamage(attackDamage, attackAxis.position, knockback);
        }

        Invoke(nameof(ReturnCanAttackState), attackCD / attackSpeedBuff);
    }

    private void BowAttack()
    {
        chargingTimer = 0;
        isCharging = true;
    }

    private void BowCharging()
    {
        if (isCharging)
        {
            chargingStrength = chargingTimer / maxChargingTime;
            chargingTimer += Time.deltaTime;
            chargingTimer = Mathf.Min(chargingTimer, maxChargingTime);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isCharging = false;

                Arrow arrow = Instantiate(this.arrow, attackAxis.position, attackAxis.rotation, GamePlayManager.instance.InstantiateObjectParent)
                    .GetComponent<Arrow>();

                arrow.damage = (int)Mathf.Round(attackDamage * chargingStrength);
                arrow.knockback = knockback * chargingStrength;
                arrow.speed *= chargingStrength;
                arrow.maxThroughEnemy = maxArrowThrough;

                Invoke(nameof(ReturnCanAttackState), attackCD / attackSpeedBuff);
            }
        }
    }

    public void SummonMinion()
    {
        Instantiate(minion, attackAxis.position, Quaternion.identity, attackAxis.parent);
    }
}
