using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ExperienceOrb : MonoBehaviour
{
    public float xp;
    public float moveSpeed;

    public GameObject getXPEffect;

    private Transform player;

    private void Start()
    {
        player = GamePlayManager.instance.player.transform;
        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        Vector3 p1 = transform.position;
        Vector3 p3 = player.position;
        Vector3 p2 = p1 + Quaternion.Euler(0, 0, Random.Range(-110f, 110f)) * (p3 - p1);

        float progress = 0f;
        while(progress < 1f)
        {
            if (GamePlayManager.instance.isCutScene || GamePlayManager.instance.isPause)
            {
                yield return null;
                continue;
            }


            progress += Time.deltaTime;
            p3 = player.position;
            Vector3 p4 = Vector3.Lerp(p1, p2, progress);
            Vector3 p5 = Vector3.Lerp(p2, p3, progress);
            transform.position = Vector3.Lerp(p4, p5, progress);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.GetXP(xp);
            Instantiate(getXPEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
