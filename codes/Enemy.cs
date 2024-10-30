using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �� ���� Ŭ����
/// </summary>
public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;  // �÷��̾�

    bool isLive;    // ����ִ��� ����

    Rigidbody2D rigid;  // enemy �ڽ�
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)   // �ð� �帧
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))    // GetCurrentAnimatorStateInfo���� ����Ǵ� �ִϸ����� ���� ����
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // ������ �������� ����� �޶����� �ʵ��� FixedDeltaTime���

        // �˹� �߿��� �ڿ������� �̵��� ���� ó��
        rigid.MovePosition(rigid.position + nextVec);

        // �˹� ȿ���� �ڿ������� �����ϵ��� ó��
        if (rigid.velocity.magnitude > speed)
        {
            rigid.velocity *= 0.95f;
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)   // �ð� �帧
            return;

        if (!isLive)    // ���� ���� ����
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    // Ȱ��ȭ�� �� �ۿ�. �׾��� ��Ƴ� ��쿡 ����.
    // PoolManager���� Instantiate�� Ȱ��ȭ �Ҷ� �ַ� ����
    private void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� ������ �ʱ� ���·� ����
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;   // ������� ������� �ǵ�����. 2 -> 1
        anim.SetBool("Dead", false);

        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
                anim.SetBool(param.name, false);
            else if (param.type == AnimatorControllerParameterType.Trigger)
                anim.ResetTrigger(param.name);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isLive = false;
        health = maxHealth;
        rigid.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
    }

    // �ʱ갪 ����
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // ����� ���� �� ����.
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(dirVec.normalized * 4, ForceMode2D.Impulse);

        HandleKnockBackEffectAsync().Forget();

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // �������� ��Ȱ��ȭ
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;   // �׾����� �ٸ� ������Ʈ�� ������ �ʵ��� ������. 2 -> 1
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive) // ������ ���� ���͸� ���� ���϶��� �Ҹ��� ���� �ʵ��� ��
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            await DeadSequenceAsync();
        }
    }

    private async UniTask HandleKnockBackEffectAsync()
    {
        float originalSpeed = speed;
        speed = 0;

        await UniTask.Delay(200);

        for (float t = 0; t < 1; t += Time.deltaTime * 2)
        {
            speed = Mathf.Lerp(0, originalSpeed, t);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        speed = originalSpeed;
        rigid.velocity = rigid.velocity.normalized * speed;
    }

    private async UniTask DeadSequenceAsync()
    {
        try
        {
            await UniTask.Delay(500);
            gameObject.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Death sequence cancelled");
        }
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}