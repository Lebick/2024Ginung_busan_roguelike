using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStone : MonoBehaviour
{
    public GameObject[] stoneObjects;

    public int spawnAmount;
    public Vector3 groundSize; // 땅 오브젝트의 크기

    void Start()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            int index = Random.Range(0, stoneObjects.Length);

            float randomX = Random.Range(-groundSize.x / 2f, groundSize.x / 2f);
            float randomZ = Random.Range(-groundSize.z / 2f, groundSize.z / 2f);

            Vector3 spawnPosition = new Vector3(randomX, 0, randomZ) + transform.position;
            Transform obj = Instantiate(stoneObjects[index], spawnPosition, Quaternion.identity, transform).transform;

            obj.localScale = new Vector3(0.05f, 1, 0.05f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, groundSize);
    }
}