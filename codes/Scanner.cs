using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지속적으로 가장 가까운 타겟 스캔
/// </summary>
public class Scanner : MonoBehaviour
{
    public float scanRange; // 범위
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // 원형태로 탐색. 파라미터 : 캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;   // 최초 거리 기준값

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos); // 거리 계산

            if(curDiff < diff)  // 가장 가까운 거리의 오브젝트를 구한다.
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

}
