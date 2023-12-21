using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }



/*    void Update() { 

        // GetAxis : 부드럽게 미끄러짐
        // GetAxisRaw : 명확한 컨트롤
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
*/
    private void FixedUpdate()
    {

        // 1. 힘을 준다.
        //rigid.AddForce(inputVec);

        // 2. 속도 제어
        //rigid.velocity = inputVec;

        // fixedDeltaTIme : 물리 프레임 하나가 소비한 시간
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; // inputVec.normalized : 대각선도 루트2가 아닌 1로 모두 정해줌

        // 3. 위치 이동 (현재위치 + 나아갈 방향)
        rigid.MovePosition(rigid.position + nextVec);

    }



    private void LateUpdate()   // 업데이터 후 다음 프레임 직전에 실행되는 함수 
    {
        anim.SetFloat("Speed", inputVec.magnitude);


        // 좌우 반전
        if (inputVec.x != 0) // key를 눌렀을 때 반응
        {
            spriter.flipX = inputVec.x < 0; // false가 들어감
        }
    }

}
