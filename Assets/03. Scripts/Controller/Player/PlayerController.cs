using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    private CircleCollider2D circleCollider;

    public PlayerAttack playerAttack;
    public PlayerHUD playerHUD;
    public GameObject levelUpUI;
    public GameObject playerSprite;

    public int level;
    public float xp;
    public int requireXP = 100;

    public int maxMP;
    public int mp;
    public float mpRecoveryInterval;
    public float canMPRecoveryWaitTime;

    public bool isCheatInvincibility;
    private bool isInvincibility;

    public float moveSpeed;
    private Vector3 moveDir;

    public float moveSpeedBuff = 1.0f;

    public ParticleSystem walkDusk;
    private ParticleSystem.MainModule walkDuskEmission;

    private bool isStun;

    public GameObject attackAxis;
    public int attackType;

    private bool isJump;
    public float jumpCD;
    private float jumpTimer;
    public float jumpLeftTime;

    public int[] skillRequireMP = new int[] {30, 20, 20, 30};
    public float[] skillCD = new float[4];
    private float[] skillTimer = new float[4];
    public float[] skillLeftTime = new float[4];
    public bool skillBind = false; //스킬 봉인 여부

    public GameObject dashEffect;
    public bool isTPtoDash;
    public bool isDashing;
    public float dashDistance = 10f;
    public int dashReturnMPValue;

    public GameObject fireBall;
    public Transform fireBallSpawnPos;

    public float teleportRange;

    public int hpRecoverySkillValue = 10;

    private bool isSummonerShield;
    public float summonerShieldCD;

    protected override void Awake()
    {
        base.Awake();
        circleCollider = GetComponent<CircleCollider2D>();

        playerAttack = GetComponent<PlayerAttack>();
        sr = playerSprite.GetComponent<SpriteRenderer>();
        anim = playerSprite.GetComponent<Animator>();
        walkDuskEmission = walkDusk.main;
    }


    private void Start()
    {
        InvokeRepeating(nameof(MPRecovery), 0, mpRecoveryInterval);

        if(DataSaveManager.instance != null)
        {
            SaveData data = DataSaveManager.instance.currentData;
            if (data != null)
            {
                level = data.level;
                attackType = data.attackType;
                moveSpeedBuff = data.moveSpeedBuff;
                isTPtoDash = data.isTPtoDash;
                dashDistance = data.dashDistance;
                dashReturnMPValue = data.dashReturnMPValue;
                playerAttack.attackDamage = data.attackDamage;
                playerAttack.attackCD = data.attackCD;
                playerAttack.normalAttackRange = data.normalAttackRange;
                playerAttack.maxChargingTime = data.maxChargingTime;
                playerAttack.maxArrowThrough = data.maxArrowThrough;
                playerAttack.minionAmount = data.minionAmount;
            }
        }
    }

    protected override void Update()
    {
        playerHUD.UpdateHUD(this);

        if (GamePlayManager.instance == null) return;

        if (GamePlayManager.instance.isCutScene)
            return;

        if (GamePlayManager.instance.isPause)
        {
            SetWalkAnim(false);
            return;
        }

        base.Update();
        if (!isStun)
        {
            UpdateMoveInput();
            UpdateAttackInput();
            UpdateSkillInput();
        }

        UpdateMouse();
        UpdateSkillLeftTime();
    }

    private void UpdateMoveInput()
    {
        Vector3 previousPos = transform.position;

        Vector3 horizontal = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 vertical = transform.up * Input.GetAxisRaw("Vertical");
        moveDir = horizontal + vertical;

        transform.position = MovePosClamp(transform.position + new Vector3(moveDir.x, 0, 0) * moveSpeed * Time.deltaTime);
        transform.position = MovePosClamp(transform.position + new Vector3(0, moveDir.y, 0) * moveSpeed * Time.deltaTime);
        
        bool isWalk = previousPos != transform.position;

        if(isWalk && !walkDusk.isPlaying)
            walkDusk.Play();

        SetWalkAnim(isWalk);
    }

    private Vector3 MovePosClamp(Vector3 nextPosition)
    {
        Vector3 dir = (nextPosition - transform.position).normalized; //방향값

        Vector3 detectOffset = dir * circleCollider.radius * 2.0f;

        Vector3 center = transform.position + (Vector3)circleCollider.offset;
        Vector3 fixPos = nextPosition + detectOffset + (Vector3)circleCollider.offset;

        Debug.DrawLine(center, fixPos, Color.red);
        RaycastHit2D hit = Physics2D.Linecast(center, fixPos, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
            return nextPosition;
        else
            return (Vector3)hit.point - detectOffset - (Vector3)circleCollider.offset;
    }

    public void SetWalkAnim(bool value)
    {
        anim.SetBool("isMove", value);
        walkDuskEmission.loop = value;
    }

    private void UpdateAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerAttack.isCanAttack)
        {
            playerAttack.Attack(attackType, knockbackStrength);
        }
    }

    private void UpdateSkillInput()
    {

        int currentSkillIndex = -1;

        if (Input.GetKeyDown(KeyCode.Mouse1))
            currentSkillIndex = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            currentSkillIndex = 1;

        if (Input.GetKeyDown(KeyCode.B))
            currentSkillIndex = 2;

        if (Input.GetKeyDown(KeyCode.R))
            currentSkillIndex = 3;

        if (currentSkillIndex == -1) return;

        if (skillLeftTime[currentSkillIndex] > 0)
        {
            playerHUD.AlertMessage("스킬 쿨다운중입니다.");
            return;
        }

        if (skillRequireMP[currentSkillIndex] > mp)
        {
            playerHUD.AlertMessage("마나가 부족합니다.");
            return;
        }

        if (skillBind)
        {
            playerHUD.AlertMessage("스킬이 봉인되어있습니다.");
            return;
        }

        mp -= skillRequireMP[currentSkillIndex];
        Invoke($"UseSkill{currentSkillIndex}", 0);

        CancelInvoke(nameof(MPRecovery));
        CancelInvoke(nameof(ReturnMPRecovery));
        Invoke(nameof(ReturnMPRecovery), canMPRecoveryWaitTime);
    }

    private void ReturnMPRecovery()
    {
        InvokeRepeating(nameof(MPRecovery), 0, mpRecoveryInterval);
    }

    private void MPRecovery()
    {
        if (GamePlayManager.instance.isCutScene) return;
        mp = Mathf.Clamp(++mp, 0, maxMP);
    }

    private void UpdateMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.z = screenPlayerPos.z;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = transform.InverseTransformDirection(worldMousePos - transform.position).normalized;

        attackAxis.transform.right = transform.right;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        attackAxis.transform.localEulerAngles = new Vector3(0, 0, angle);

        sr.flipX = dir.x < 0;
    }

    private void UpdateSkillLeftTime()
    {
        if (skillBind) return;

        for (int i = 0; i < 4; i++)
        {
            skillTimer[i] -= Time.deltaTime;
            skillLeftTime[i] = skillTimer[i] / skillCD[i];
        }
    }

    public override void GetDamage(int damage, Vector3 hitObjectPos, float knockback)
    {
        if (isCheatInvincibility || isInvincibility || isStun) return;

        if (isSummonerShield)
        {
            isSummonerShield = false;
            return;
        }

        base.GetDamage(damage, hitObjectPos, knockback);

        TimeScale.instance.SetTimeScale(0.1f, 0.15f);

        isStun = true;
        Invoke(nameof(ReturnStunState), 0.5f);
    }

    private void ReturnStunState()
    {
        isStun = false;
    }

    public void SetInvincibleState()
    {
        isCheatInvincibility = !isCheatInvincibility;
    }

    public void GetInvincibleState()
    {
        isInvincibility = true;

        if(IsInvoking(nameof(ReturnInvincibleState)))
            CancelInvoke(nameof(ReturnInvincibleState));

        Invoke(nameof(ReturnInvincibleState), 5);
    }

    private void ReturnInvincibleState()
    {
        isInvincibility = false;
    }

    public void SetSummonerShield()
    {
        if (IsInvoking(nameof(GetSummonerShield)))
            CancelInvoke(nameof(GetSummonerShield));

        InvokeRepeating(nameof(GetSummonerShield), 0, summonerShieldCD);
    }

    private void GetSummonerShield()
    {
        isSummonerShield = true;
    }

    public void SetSkillBind()
    {
        if (IsInvoking(nameof(ReturnSkillBind)))
            CancelInvoke(nameof(ReturnSkillBind));

        skillBind = true;
        Invoke(nameof(ReturnSkillBind), 5f);
    }

    private void ReturnSkillBind()
    {
        skillBind = false;
    }

    private void UseSkill0()
    {
        skillTimer[0] = skillCD[0];
        Instantiate(fireBall, fireBallSpawnPos.position, attackAxis.transform.rotation);
        GetKnockback((transform.position - fireBallSpawnPos.position).normalized, 10f, 0.3f);
    }

    private void UseSkill1()
    {
        skillTimer[1] = skillCD[1];

        if (isTPtoDash)
            Dash();
        else
            transform.position = MovePosClamp(transform.position + moveDir * teleportRange);
    }

    private void Dash()
    {
        isDashing = true;
        dashEffect.SetActive(true);
        GetKnockback(moveDir, dashDistance, 0.3f);
        Invoke(nameof(ReturnDash), 0.3f);
    }

    private void ReturnDash()
    {
        isDashing = false;
    }

    private void UseSkill2()
    {
        skillTimer[2] = skillCD[2];

        playerAttack.currentAttackSpeedBuff = playerAttack.attackSpeedBuff;
        Invoke(nameof(ReturnSkill2), playerAttack.attackBuffDuration);
    }

    private void ReturnSkill2()
    {
        playerAttack.currentAttackSpeedBuff = 1.0f;
    }

    private void UseSkill3()
    {
        skillTimer[3] = skillCD[3];
        hp += hpRecoverySkillValue;
    }

    public void GetXP(float value)
    {
        xp += value;
        if (xp >= requireXP)
        {
            LevelUP();
            xp -= requireXP;
        }
    }

    public void LevelUP()
    {
        level++;
        GamePlayManager.instance.isPause = true;
        Instantiate(levelUpUI, GamePlayManager.instance.overlayCanvas);
        Time.timeScale = 0f;
    }

    protected override void OnDeath()
    {
        GamePlayManager.instance.GameOver(new SaveData(level, attackType, moveSpeedBuff, isTPtoDash, dashDistance, dashReturnMPValue, playerAttack.attackDamage, playerAttack.attackCD,
            playerAttack.normalAttackRange, playerAttack.maxChargingTime, playerAttack.maxArrowThrough, playerAttack.minionAmount));

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        foreach(MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            if(script == this)
                script.enabled = false;
        }
        this.enabled = false;

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, teleportRange);
    }
}