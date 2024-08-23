using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSniperEnemy : EnemyController
{
    public SniperEnemyAttack attackObj;

    public override void Attack()
    {
        SniperEnemyAttack obj = Instantiate(attackObj, transform.position, transform.rotation, GamePlayManager.instance.InstantiateObjectParent);
        obj.damage = attackDamage;
        obj.knockback = knockbackStrength;
    }
}
