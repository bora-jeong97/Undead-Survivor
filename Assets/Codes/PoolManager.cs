using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹 보관 변수
    public GameObject[] prefabs;

    // 풀 담당 리스트들
    List<GameObject>[] pools;

    private void Awake()
    {
        // 프리팹의 수만큼 list생성 1대1
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // 선택한 풀의 비활성화된 게임 오브젝트 접근
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)   // 비활성화된 object가 있다면
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true); // 그것을 활성화
                break;
            }
        }
        // 못 찾을 시 
        if (!select)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);    // poolManager안에 넣음
            pools[index].Add(select);

        }

        return select;
    }
}
