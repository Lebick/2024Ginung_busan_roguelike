using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Boss : MonoBehaviour
{
    /*
     * 특정 주기 or 특정 조건에 따라 게임에 존재하던 적 중 랜덤한 적으로 변함.
     * 보스로 나왔던 적으로는 변하지 않는다.
     * 반드시 텔레포트로만 움직임. (저주몬스터 제외)
     * 스나이핑 적의 경우, 1회 공격 후 텔레포트를 하며, 3회공격시 다른적으로 변한다.
     * 박쥐의 경우, 근처에 다른 박쥐를 5마리 소환한다.
     * 저주 몬스터의 경우, 플레이어에게 닿았거나 5초이상 닿지 못했다면 다른 적으로 변한다.
     * 힐러로 변할 시 멈춰서 자신의 체력을 10초간 회복하며, 회복도중 공격받을 시 회복한 체력이 초기화되며, 그로기상태가 된다.
     * 힐을 시전하기 전, 플레이어로부터 가장 먼 스폰위치로 이동한다.
     * 그로기상태 해제 or 체력 회복 종료시 다른 적으로 변함.
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
