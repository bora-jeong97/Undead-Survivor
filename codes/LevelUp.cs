using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 레벨업 시 나타나는 ui창 기능 제어
/// </summary>
public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);    // 비활성화된 자식도 가져와야하기 때문에 true
    }

    /// <summary>
    /// 스킬 선택 창 보이기 및 시간 정지
    /// </summary>
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    /// <summary>
    /// 스킬 선택 창 보이기 및 시간 흐름
    /// </summary>
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        
    }

    /// <summary>
    /// 초기 기본 무기 지급
    /// </summary>
    public void Select(int index)
    {
        items[index].OnClick();
    }

    /// <summary>
    /// 스킬 창 랜덤 알고리즘
    /// </summary>
    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            // 모두 같지 않을 때 나감
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for(int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            // 3. 만렙 아이템의 경우는 소비아이템으로 대체
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
                // items[Random.Range(4, 7)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);

            }
        }

    }
}
