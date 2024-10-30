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
    public Hand[] hands;    // 손에 든 무기
    public RuntimeAnimatorController[] animCon;
    public GameObject postProcessing; // 타격시 가시적 효과

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);    // true로 넣으면 비활성화된 오브젝트도 컴포넌트를 받아 올 수 있다.
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
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

        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;  // inputVec.normalized : 대각선도 루트2가 아닌 1로 같은 속도로 유지하기 위함
        rigid.MovePosition(rigid.position + nextVec);

        if (GameManager.instance.health < GameManager.instance.maxHealth / 2)   // 체력이 절반 이하일 시 가시적 효과
        {
            postProcessing.SetActive(true);
        } else
        {
            postProcessing.SetActive(false);
        }
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
    /// 몬스터 충돌 지속시 발생하는 피해
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10; // 초당 -10
       


        if (GameManager.instance.health < 0)
        {
            for(int index=2; index < transform.childCount; index++) // 플레이어와 그림자 외 나머지 자식 오브젝트 비활성화
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    #region ragacy 체력 30% 이하일 때로 기획 변경
    /// <summary>
    /// 타격을 입은 wait 시간동안만 활성화
    /// </summary>
    /// <returns></returns>
/*    IEnumerator PostProcessingRoutine()
    {
        Debug.Log("PostProcessingRoutine실행");
        postProcessing.SetActive(true);
        yield return wait;
        postProcessing.SetActive(false);
        Debug.Log("PostProcessingRoutine실행완료");

    }

     private async UniTask PostProcessingRoutineAsync()
    {
        Debug.Log("PostProcessingRoutine실행");
        postProcessing.SetActive(true);
        // yield return wait; // WaitForFixedUpdate 대신 UniTask.Yield 사용
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        postProcessing.SetActive(false);
        Debug.Log("PostProcessingRoutine실행완료");
    }
 
 */
    #endregion



    // InputSystem 활용
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();    // nomalized를 이미 사용하고 있음.
    }

}
