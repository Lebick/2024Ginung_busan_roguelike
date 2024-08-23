using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Boss : MonoBehaviour
{
    /*
     * Ư�� �ֱ� or Ư�� ���ǿ� ���� ���ӿ� �����ϴ� �� �� ������ ������ ����.
     * ������ ���Դ� �����δ� ������ �ʴ´�.
     * �ݵ�� �ڷ���Ʈ�θ� ������. (���ָ��� ����)
     * �������� ���� ���, 1ȸ ���� �� �ڷ���Ʈ�� �ϸ�, 3ȸ���ݽ� �ٸ������� ���Ѵ�.
     * ������ ���, ��ó�� �ٸ� ���㸦 5���� ��ȯ�Ѵ�.
     * ���� ������ ���, �÷��̾�� ��Ұų� 5���̻� ���� ���ߴٸ� �ٸ� ������ ���Ѵ�.
     * ������ ���� �� ���缭 �ڽ��� ü���� 10�ʰ� ȸ���ϸ�, ȸ������ ���ݹ��� �� ȸ���� ü���� �ʱ�ȭ�Ǹ�, �׷α���°� �ȴ�.
     * ���� �����ϱ� ��, �÷��̾�κ��� ���� �� ������ġ�� �̵��Ѵ�.
     * �׷α���� ���� or ü�� ȸ�� ����� �ٸ� ������ ����.
     */

    public int maxHP;
    private int _hp;
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value > maxHP)
                _hp = maxHP;
            else
                _hp = value;
        }
    }

    public float recoveryHP;

    public PlayerController player;

    public List<EnemyController> transformationEnemys;
    public int currentEnemyIndex = 0;
    public EnemyController currentEnemyObj;

    public Transform transformationEffect;

    public bool isTransformation;

    private bool bossStart;

    private void Start()
    {
        hp = maxHP;
        player = GamePlayManager.instance.player;
        BossStart();
        Transformation();
    }

    private void BossStart()
    {
        bossStart = true;
        GameHUD.instance.bossCanvasGroup.Alpha0to1();
    }

    private void Update()
    {
        if (!bossStart) return;

        GameHUD.instance.UpdateBossHPValue(maxHP, hp);
        GameHUD.instance.UpdateBoss3HPValue(maxHP, hp + recoveryHP);
    }

    public void SetTransformation()
    {
        if (isTransformation) return;

        isTransformation = true;
        hp -= (int)(maxHP * 0.1f);

        transformationEffect.position = currentEnemyObj.transform.position;
        transformationEffect.GetComponent<ParticleSystem>().Play();

        Invoke(nameof(Transformation), 1f);
    }

    private void Transformation()
    {
        int newEnemy = Random.Range(0, transformationEnemys.Count);
        while (newEnemy == currentEnemyIndex)
            newEnemy = Random.Range(0, transformationEnemys.Count);

        EnemyController newObj = Instantiate(transformationEnemys[newEnemy], currentEnemyObj.transform.position, Quaternion.Euler(0, 0, GamePlayManager.instance.currentRotation), transform);

        Destroy(currentEnemyObj.gameObject);

        currentEnemyObj = newObj;
        isTransformation = false;
        currentEnemyIndex = newEnemy;
    }

    public void GetHeal()
    {
        hp += (int)recoveryHP;
        recoveryHP = 0f;
    }
}
