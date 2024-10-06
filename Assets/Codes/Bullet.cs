using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 주위를 도는 bullet 무기 
/// </summary>
public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // 관통


    Rigidbody2D rigid;  // 원거리 무기를 위한 물리작용

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // 관통이 -1(무한)보다 큰 것에 대해서는 원거리 무기로 판정. 속도 적용
        if(per > -1)
        {
            rigid.velocity = dir * 15f; // 속도 = 방향 * 속력
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--;  // 한 마리에 부딪치면 관통력이 줄어든다

        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);    // 오브젝트 풀링에서 재활용할 것
        }
    }
}
