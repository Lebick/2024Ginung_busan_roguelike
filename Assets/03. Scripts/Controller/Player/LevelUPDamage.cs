using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUPDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<EnemyController>(out EnemyController enemy))
            enemy.ForceDeath();
    }
}
