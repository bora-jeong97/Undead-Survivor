using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 적 제어 클래스
/// </summary>
public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;  // 플레이어

    bool isLive;    // 살아있는지 여부

    Rigidbody2D rigid;  // enemy 자신
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
        if (!GameManager.instance.isLive)   // 시간 흐름
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))    // GetCurrentAnimatorStateInfo현재 실행되는 애니메이터 실행 정보
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // 프레임 영향으로 결과가 달라지지 않도록 FixedDeltaTime사용

        // 넉백 중에도 자연스러운 이동을 위한 처리
        rigid.MovePosition(rigid.position + nextVec);

        // 넉백 효과가 자연스럽게 감소하도록 처리
        if (rigid.velocity.magnitude > speed)
        {
            rigid.velocity *= 0.95f;
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)   // 시간 흐름
            return;

        if (!isLive)    // 몬스터 생사 여부
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 활성화될 시 작용. 죽었다 살아날 경우에 쓰임.
    // PoolManager에서 Instantiate로 활성화 할때 주로 쓰임
    private void OnEnable()
    {
        // 오브젝트가 활성화될 때마다 초기 상태로 리셋
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;   // 살았으니 원래대로 되돌린다. 2 -> 1
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

    // 초깃값 지정
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // 무기와 만날 때 적용.
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
            // 죽음으로 비활성화
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;   // 죽었으니 다른 오브젝트를 가리지 않도록 내린다. 2 -> 1
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive) // 마지막 게임 몬스터를 전부 죽일때는 소리가 나지 않도록 함
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