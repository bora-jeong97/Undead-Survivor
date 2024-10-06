using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 움직임 및 애니메이션
/// </summary>
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();

        
    }

    #region Legacy
/*    private void Update()
    {
        // GetAxis : 부드러움
        // GetAxisRaw : 경직된 움직임
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }*/
    #endregion

    private void FixedUpdate()
    {
        #region Legacy
/*        // 1. 힘을 준다
        rigid.AddForce(inputVec);

        // 2. 속도 제어
        rigid.velocity = inputVec;*/
        #endregion

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;  // inputVec.normalized : 대각선도 루트2가 아닌 1로 같은 속도로 유지하기 위함
        rigid.MovePosition(rigid.position + nextVec);
    }
    
    // InputSystem 활용
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();    // nomalized를 이미 사용하고 있음.
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
