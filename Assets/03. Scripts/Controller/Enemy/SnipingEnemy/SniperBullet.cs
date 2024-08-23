using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public int damage;
    public float knockback;

    private float lookSpeed = 0;

    private Vector3 targetPos;

    public GameObject warningObj;

    private GameObject instantiateWarning;

    private PlayerController player;
    private void Start()
    {
        player = GamePlayManager.instance.player;
        targetPos = GamePlayManager.instance.player.transform.position;
        instantiateWarning = Instantiate(warningObj, targetPos, transform.parent.rotation, GamePlayManager.instance.rotationObj);
    }

    private void Update()
    {
        if (GamePlayManager.instance.isCutScene || GamePlayManager.instance.isPause) return;

        lookSpeed += Time.deltaTime / 5f;

        lookSpeed = Mathf.Min(lookSpeed, 1);

        Vector2 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.Translate(Vector3.up * Time.deltaTime * 10f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), lookSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(instantiateWarning))
        {
            CheckPlayer();
            Destroy(instantiateWarning);
            Destroy(gameObject);
        }
    }

    private void CheckPlayer()
    {
        BoxCollider2D box = instantiateWarning.GetComponent<BoxCollider2D>();

        if (Physics2D.OverlapBox((Vector2)box.transform.position + box.offset, box.size, GamePlayManager.instance.currentRotation, LayerMask.GetMask("Player")))
        {
            player.GetDamage(damage, transform.position, knockback);
        }
    }
}
