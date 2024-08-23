using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemyAttack : MonoBehaviour
{
    public SniperBullet[] horns;
    public int damage;
    public float knockback;

    private void Start()
    {
        foreach(SniperBullet horn in horns)
        {
            horn.damage = damage;
            horn.knockback = knockback;
        }
    }

    private void Update()
    {
        if (transform.childCount == 0) Destroy(gameObject); 
    }
}
