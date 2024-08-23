using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stage1Boss : EnemyController
{
    public bool isActing; //다른 상태로 변경되지 못하는 상태 (공격중, 텔레포트중 등의 상황)

    public float maxFollowTime; //해당 시간동안 움직였음에도 범위에 플레이어가 없을 시 순간이동함.
    public float followTime; //플레이어를 따라 움직인 시간
    private int teleportCount; //플레이어가 계속 도망갈 때 강제로 스킬을 발동하기 위함

    public GameObject stoneObj;
    public GameObject stoneDestroyEffect;
    public float stoneAttackRange;
    public int stoneDamage;

    public ParticleSystem pullingEffect;
    public float pullingRange;
    public float pullingAfterPoint;
    public GameObject[] pullingWarning;
    public GameObject[] pullingDamageEffect;
    public float[] pullingDamageRange;

    private bool bossStart;

    private void Start()
    {
        player = GamePlayManager.instance.player;
        Invoke(nameof(BossStart), 5f);
    }

    private void BossStart()
    {
        bossStart = true;
        GameHUD.instance.bossCanvasGroup.Alpha0to1();
    }

    protected override void Update()
    {
        if (!bossStart) return;

        GameHUD.instance.UpdateBossHPValue(maxHP, hp);

        anim.SetBool("IsMove", !isActing);

        if (!isActing)
            SetState();
    }

    private void SetState()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > detectRange) //거리가 멀다면
        {
            if (followTime < maxFollowTime)     //최대 이동 시간을 넘지 않았다면
            {
                FollowPlayer();                 //걸어감
            }
            else                                //최대 이동 시간을 넘었다면
            {
                if (teleportCount > 3) //3번 텔레포트 할 동안 계속해서 플레이어가 도망갔다면
                {
                    UseSkill(); //그냥 스킬 사용
                    return;
                }

                isActing = true;
                StartCoroutine(Skill0()); //텔레포트
                teleportCount++;
            }

            followTime += Time.deltaTime; //이동한 시간을 추가
        }
        else //범위 내에 들어왔다면
        {
            UseSkill();
        }
    }

    private void UseSkill()
    {
        isActing = true;
        followTime = 0; //이동한 시간을 초기화
        teleportCount = 0; //텔레포트한 횟수를 초기화

        int randomAttack = Random.Range(1, 3); //돌떨구기, 끌어당기기 둘중 랜덤 1개 패턴
        StartCoroutine($"Skill{randomAttack}");
    }

    private IEnumerator Skill0() //텔레포트
    {
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);

        Vector3 teleportPos = player.attackAxis.transform.position - player.attackAxis.transform.right * 1.3f;
        transform.position = teleportPos;

        yield return new WaitForSeconds(0.6f);
        isActing = false;
    }

    private IEnumerator Skill1() //돌떨구기
    {
        anim.SetTrigger("StoneAttack");
        yield return new WaitForSeconds(0.7f);
        CameraShakeManager.instance.StartShake(0.05f, 0.5f);

        for(int i=0; i<50; i++)
        {
            StartCoroutine(DropStone());
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        isActing = false;
    }

    private IEnumerator DropStone()
    {
        Vector3 randomPos = (Vector2)player.transform.position + Random.insideUnitCircle * stoneAttackRange;
        GameObject obj = Instantiate(stoneObj, randomPos, Quaternion.Euler(0, 0, GamePlayManager.instance.currentRotation), GamePlayManager.instance.rotationObj);
        yield return new WaitForSeconds(0.83f);
        CheckPlayer(obj);
        Destroy(obj);
        Instantiate(stoneDestroyEffect, randomPos, Quaternion.Euler(0, 0, GamePlayManager.instance.currentRotation), GamePlayManager.instance.rotationObj);
    }

    private void CheckPlayer(GameObject obj)
    {
        BoxCollider2D box = obj.GetComponent<BoxCollider2D>();

        if (Physics2D.OverlapBox((Vector2)box.transform.position + box.offset, box.size, GamePlayManager.instance.currentRotation, LayerMask.GetMask("Player")))
        {
            player.GetDamage(stoneDamage, transform.position, knockbackStrength);
        }
    }

    private IEnumerator Skill2() //끌당
    {
        pullingEffect.Play();
        ParticleSystem.ShapeModule test = pullingEffect.shape;
        test.radius = 3;
        float progress = 0f;

        while(progress < 2)
        {
            progress += Time.deltaTime;
            test.radius += progress / 5f;
            yield return null;
        }

        //플레이어가 끌어당기는 범위 내에 없었다면 코루틴 취소
        if (Physics2D.OverlapCircle(transform.position, pullingRange, LayerMask.GetMask("Player")) == null)
        {
            yield return new WaitForSeconds(1f);
            isActing = false;
            yield break;
        }

        progress = 0f;
        Vector3 pos = player.transform.position;
        Vector3 targetPos = transform.position + (pos - transform.position).normalized * pullingAfterPoint;

        //끌어당김
        while (progress <= 1f)
        {
            progress += Time.deltaTime * 10f;
            player.transform.position = Vector3.Lerp(pos, targetPos, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        int randomWarning = Random.Range(0, 2);

        //위험표시
        pullingWarning[randomWarning].SetActive(true);
        yield return new WaitForSeconds(0.75f);
        pullingWarning[randomWarning].SetActive(false);
        pullingDamageEffect[randomWarning].SetActive(true);

        progress = 0f;

        //4초간 공격범위에있는 플레이어에게 데미지를 줌
        while(progress <= 4f)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if((randomWarning == 0 && distance <= pullingDamageRange[0])
            || (randomWarning == 1 && distance > pullingDamageRange[0] && distance <= pullingDamageRange[1]))
            {
                player.GetDamage(5, transform.position, knockbackStrength);
            }
            progress += Time.deltaTime;
            yield return null;
        }

        isActing = false;
        yield return null;
    }

    private IEnumerator Skill3() //넓은 파장 (반드시 점프로 피해야함)
    {
        
        yield return null;
    }

    //피해 반사도 하나 넣으면 좋을듯?


    public override void GetDamage(int damage, Vector3 hitObjectPos, float knockback)
    {
        base.GetDamage(damage, hitObjectPos, knockback);
    }

    protected override void OnDeath()
    {
        GamePlayManager.instance.stageSequence.ClearSequence();
        base.OnDeath();
    }

    public override void ForceDeath(){ }

    public override void GetKnockback(Vector2 dir, float knockback, float knockbackReturnTime = 0.5f){ }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, stoneAttackRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, pullingRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, pullingAfterPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullingDamageRange[0]);
        Gizmos.DrawWireSphere(transform.position, pullingDamageRange[1]);
    }
}
