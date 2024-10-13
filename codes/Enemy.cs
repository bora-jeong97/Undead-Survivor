using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    WaitForFixedUpdate wait;    // 다음 fixed update까지 기다린다

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)   // 시간 흐름
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))    // GetCurrentAnimatorStateInfo현재 실행되는 애니메이터 실행 정보
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // 프레임 영향으로 결과가 달라지지 않도록 FixedDeltaTime사용
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리속도가 이동에 영향 주지 않도록 0으로 준다. 부딪쳤을때 속도가 빨라져서 밀려나지 않도록 함.
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
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;   // 살았으니 원래대로 되돌린다. 2 -> 1
        anim.SetBool("Dead", false);
        health = maxHealth;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());    // 넉백
        

        if(health > 0)
        {
            anim.SetTrigger("Hit");
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
        }
    }

    // 타격시 넉백 
    IEnumerator KnockBack()
    {
        yield return wait;  // 다음 하나의 물리 프레임까지 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;    // 플레이어 기준 반대 방향
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 3만큼 반대방향으로 힘을 준다
    }

    // Animations/Enemy/DeadEnemy 코드가 아닌 애니메이션 이벤트 시스템을 이용해 직접 호출
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
