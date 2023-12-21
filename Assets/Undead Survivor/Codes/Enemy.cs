using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter; 

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {

        if (!isLive)
            return;


        // 위치 차이 = 타겟 위치 - 나의 위치
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;  // 플레이어와 부딪쳐도 이동하지 않도록
        // 방향 = 위치 차이의 정규화 (Normalized)
        // nextVec : 플레이어의 키입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
    }


    private void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }
}
