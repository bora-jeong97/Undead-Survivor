using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� ������ �� �ִϸ��̼�
/// </summary>
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;    // �տ� �� ����
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);    // true�� ������ ��Ȱ��ȭ�� ������Ʈ�� ������Ʈ�� �޾� �� �� �ִ�.


    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }


    #region Legacy
    /*    private void Update()
        {
            // GetAxis : �ε巯��
            // GetAxisRaw : ������ ������
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");
        }*/
    #endregion

    private void FixedUpdate()
    {
        #region Legacy
        /*        // 1. ���� �ش�
                rigid.AddForce(inputVec);

                // 2. �ӵ� ����
                rigid.velocity = inputVec;*/
        #endregion

        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;  // inputVec.normalized : �밢���� ��Ʈ2�� �ƴ� 1�� ���� �ӵ��� �����ϱ� ����
        rigid.MovePosition(rigid.position + nextVec);
    }
    
    // InputSystem Ȱ��
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();    // nomalized�� �̹� ����ϰ� ����.
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    /// <summary>
    /// ���� �浹 ���ӽ� �߻��ϴ� ����
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10; // �ʴ� -10

        if(GameManager.instance.health < 0)
        {
            for(int index=2; index < transform.childCount; index++) // �÷��̾�� �׸��� �� ������ �ڽ� ������Ʈ ��Ȱ��ȭ
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
