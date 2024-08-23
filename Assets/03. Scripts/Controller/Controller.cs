using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{
    public GameObject damageEffect;
    public GameObject deathEffect;
    protected SpriteRenderer sr;
    protected Rigidbody2D rigid;
    protected Animator anim;

    public int maxHP;
    public int hp;
    public float knockbackStrength;

    protected virtual void Awake()
    {
        hp = maxHP;
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        ClampValue();
        ClampVelocity();
    }

    protected virtual void ClampValue()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
    }

    protected virtual void ClampVelocity()
    {
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, 5f);
    }

    public virtual void GetDamage(int damage, Vector3 hitObjectPos = new Vector3(), float knockback = 0)
    {
        hp -= damage;
        
        Quaternion rotDirection = Quaternion.LookRotation(transform.position - hitObjectPos);
        Vector2 direction = (transform.position - hitObjectPos).normalized;

        GetKnockback(direction, knockback);

        Instantiate(damageEffect, transform.position, rotDirection);

        SetWhite();

        if (hp <= 0)
            OnDeath();
    }

    public virtual void GetKnockback(Vector2 dir, float knockback, float knockbackReturnTime = 0.5f)
    {
        rigid.AddForce(dir * knockback, ForceMode2D.Impulse);

        StopCoroutine(SetVelocityZero(knockbackReturnTime));
        StartCoroutine(SetVelocityZero(knockbackReturnTime));
    }

    private void SetWhite()
    {
        Material newMaterial = new(sr.material);
        newMaterial.SetFloat("_IsColor", 1);
        sr.material = newMaterial;

        Invoke(nameof(ReturnWhite), 0.2f);
    }

    private void ReturnWhite()
    {
        Material newMaterial = new(sr.material);
        newMaterial.SetFloat("_IsColor", 0);
        sr.material = newMaterial;
    }

    private IEnumerator SetVelocityZero(float returnTime)
    {
        Vector2 startValue = rigid.velocity;
        float progress = 0;
        while(progress < 1f)
        {
            progress += Time.deltaTime / returnTime;
            rigid.velocity = Vector2.Lerp(startValue, Vector2.zero, progress);
            yield return null;
        }
    }

    protected virtual void OnDeath()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}