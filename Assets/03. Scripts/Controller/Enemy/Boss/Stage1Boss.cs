using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stage1Boss : EnemyController
{
    public bool isActing; //�ٸ� ���·� ������� ���ϴ� ���� (������, �ڷ���Ʈ�� ���� ��Ȳ)

    public float maxFollowTime; //�ش� �ð����� ������������ ������ �÷��̾ ���� �� �����̵���.
    public float followTime; //�÷��̾ ���� ������ �ð�
    private int teleportCount; //�÷��̾ ��� ������ �� ������ ��ų�� �ߵ��ϱ� ����

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
        if (Vector3.Distance(player.transform.position, transform.position) > detectRange) //�Ÿ��� �ִٸ�
        {
            if (followTime < maxFollowTime)     //�ִ� �̵� �ð��� ���� �ʾҴٸ�
            {
                FollowPlayer();                 //�ɾ
            }
            else                                //�ִ� �̵� �ð��� �Ѿ��ٸ�
            {
                if (teleportCount > 3) //3�� �ڷ���Ʈ �� ���� ����ؼ� �÷��̾ �������ٸ�
                {
                    UseSkill(); //�׳� ��ų ���
                    return;
                }

                isActing = true;
                StartCoroutine(Skill0()); //�ڷ���Ʈ
                teleportCount++;
            }

            followTime += Time.deltaTime; //�̵��� �ð��� �߰�
        }
        else //���� ���� ���Դٸ�
        {
            UseSkill();
        }
    }

    private void UseSkill()
    {
        isActing = true;
        followTime = 0; //�̵��� �ð��� �ʱ�ȭ
        teleportCount = 0; //�ڷ���Ʈ�� Ƚ���� �ʱ�ȭ

        int randomAttack = Random.Range(1, 3); //��������, ������� ���� ���� 1�� ����
        StartCoroutine($"Skill{randomAttack}");
    }

    private IEnumerator Skill0() //�ڷ���Ʈ
    {
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);

        Vector3 teleportPos = player.attackAxis.transform.position - player.attackAxis.transform.right * 1.3f;
        transform.position = teleportPos;

        yield return new WaitForSeconds(0.6f);
        isActing = false;
    }

    private IEnumerator Skill1() //��������
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

    private IEnumerator Skill2() //����
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

        //�÷��̾ ������� ���� ���� �����ٸ� �ڷ�ƾ ���
        if (Physics2D.OverlapCircle(transform.position, pullingRange, LayerMask.GetMask("Player")) == null)
        {
            yield return new WaitForSeconds(1f);
            isActing = false;
            yield break;
        }

        progress = 0f;
        Vector3 pos = player.transform.position;
        Vector3 targetPos = transform.position + (pos - transform.position).normalized * pullingAfterPoint;

        //������
        while (progress <= 1f)
        {
            progress += Time.deltaTime * 10f;
            player.transform.position = Vector3.Lerp(pos, targetPos, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        int randomWarning = Random.Range(0, 2);

        //����ǥ��
        pullingWarning[randomWarning].SetActive(true);
        yield return new WaitForSeconds(0.75f);
        pullingWarning[randomWarning].SetActive(false);
        pullingDamageEffect[randomWarning].SetActive(true);

        progress = 0f;

        //4�ʰ� ���ݹ������ִ� �÷��̾�� �������� ��
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

    private IEnumerator Skill3() //���� ���� (�ݵ�� ������ ���ؾ���)
    {
        
        yield return null;
    }

    //���� �ݻ絵 �ϳ� ������ ������?


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
