using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ������ ���� ����
    public GameObject[] prefabs;

    // Ǯ ��� ����Ʈ��
    List<GameObject>[] pools;

    private void Awake()
    {
        // �������� ����ŭ list���� 1��1
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // ������ Ǯ�� ��Ȱ��ȭ�� ���� ������Ʈ ����
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)   // ��Ȱ��ȭ�� object�� �ִٸ�
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true); // �װ��� Ȱ��ȭ
                break;
            }
        }
        // �� ã�� �� 
        if (!select)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);    // poolManager�ȿ� ����
            pools[index].Add(select);

        }

        return select;
    }
}
