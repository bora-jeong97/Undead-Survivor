using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ �� ��Ÿ���� uiâ ��� ����
/// </summary>
public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);    // ��Ȱ��ȭ�� �ڽĵ� �����;��ϱ� ������ true
    }

    /// <summary>
    /// ��ų ���� â ���̱� �� �ð� ����
    /// </summary>
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    /// <summary>
    /// ��ų ���� â ���̱� �� �ð� �帧
    /// </summary>
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);

    }

    /// <summary>
    /// �ʱ� �⺻ ���� ����
    /// </summary>
    public void Select(int index)
    {
        items[index].OnClick();
    }

    /// <summary>
    /// ��ų â ���� �˰���
    /// </summary>
    void Next()
    {
        // 1. ��� ������ ��Ȱ��ȭ
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. �� �߿��� ���� 3�� ������ Ȱ��ȭ
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            // ��� ���� ���� �� ����
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for(int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            // 3. ���� �������� ���� �Һ���������� ��ü
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
