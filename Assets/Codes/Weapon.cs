using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 웨폰 오브젝트가 갖는 스크립트. 플레이어가 다룰 무기 배치, 초기화 관리 클래스
/// </summary>
public class Weapon : MonoBehaviour
{
    public int id;  // 웨폰 유니크 아이디
    public int prefabId;    // PoolManager에 등록된 prefab id
    public float damage;
    public int count;
    public float speed;

    float timer; // 일정 간격을 두고 원거리 무기 발사
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();   // 부모 오브젝트의 컴포넌트를 가져온다
    }


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (id)
        {
            case 0: // 회전 무기
                // 회전 한다
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:    // 원거리 무기
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1); 
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id == 0)
        {
            Batch();
        }
    }


    /// <summary>
    /// id별 각각 초깃값
    /// </summary>
    public void Init()
    {
        switch (id)
        {
            case 0: // 회전 무기
                speed = 150;    // 회전 속도
                Batch();
                break;
            default:
                // 원거리 무기
                speed = 0.3f;   // 연사 속도
                break;
        }
    }




    /// <summary>
    /// 무기 생성시 위치 배치
    /// </summary>
    void Batch()
    {
        for (int index =0; index < count; index++)
        {
            
            Transform bullet;
            
            // 기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기 
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  // 부모 오브젝트의 위치를 재지정 Weapon 휘하로 둠.
            }
            

            // 생성 로컬 위치 초기화 
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count; // 360도를 개수만큼 간격을 나눠서 돌게 한다.
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World); // local 플레이어의 y높이로 1.5f거리로 두고 이동방향은 world기준
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity per.
        }
    }

    
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;   // 거리 : 크기와 방향
        dir = dir.normalized; // normalized : 현재 백터의 방향은 유지하고 크기를 1로 변환하는 속성

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // count : 관통력

    }

}
