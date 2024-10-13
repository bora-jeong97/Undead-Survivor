using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    WaitForFixedUpdate wait;    // ���� fixed update���� ��ٸ���

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
        if (!GameManager.instance.isLive)   // �ð� �帧
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))    // GetCurrentAnimatorStateInfo���� ����Ǵ� �ִϸ����� ���� ����
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // ������ �������� ����� �޶����� �ʵ��� FixedDeltaTime���
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // �����ӵ��� �̵��� ���� ���� �ʵ��� 0���� �ش�. �ε������� �ӵ��� �������� �з����� �ʵ��� ��.
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
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;   // ������� ������� �ǵ�����. 2 -> 1
        anim.SetBool("Dead", false);
        health = maxHealth;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());    // �˹�
        

        if(health > 0)
        {
            anim.SetTrigger("Hit");
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
        }
    }

    // Ÿ�ݽ� �˹� 
    IEnumerator KnockBack()
    {
        yield return wait;  // ���� �ϳ��� ���� �����ӱ��� ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;    // �÷��̾� ���� �ݴ� ����
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 3��ŭ �ݴ�������� ���� �ش�
    }

    // Animations/Enemy/DeadEnemy �ڵ尡 �ƴ� �ִϸ��̼� �̺�Ʈ �ý����� �̿��� ���� ȣ��
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
