using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ������ ���� bullet ���� 
/// </summary>
public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // ����


    Rigidbody2D rigid;  // ���Ÿ� ���⸦ ���� �����ۿ�

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // ������ 0 ���� ū �Ϳ� ���ؼ��� ���Ÿ� ����� ����. �ӵ� ����
        if(per >= 0)
        {
            rigid.velocity = dir * 15f; // �ӵ� = ���� * �ӷ�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;  // �� ������ �ε�ġ�� ������� �پ���

        if(per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);    // ������Ʈ Ǯ������ ��Ȱ���� ��
        }
    }

    /// <summary>
    /// ���Ÿ� ���Ⱑ Area ������ ������ ��Ȱ��ȭ
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);   
    }
}
