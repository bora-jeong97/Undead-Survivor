using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� 
/// </summary>
public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;  // Ÿ��
    public float rate;  // ��ġ

    /// <summary>
    /// id�� ���� �ʱ갪
    /// </summary>
    public void Init(ItemData data)
    {
        // Basic set
        name = "Gear" + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;


        // Property set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }


    /// <summary>
    /// �尩 ������ : ������ �ӵ��� �ø��� �Լ�
    /// </summary>
    void RateUp()
    {
        
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();  // �θ� ������Ʈ�� �ö󰡼� ��� weapon��������

        // ��� ���� �ӵ� up
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // ��������
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default: // ���Ÿ� ��
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);  // �߻� ����
                    break;
            }
        }
    }

    /// <summary>
    /// ��ȭ ������ : �÷��̾��� �ӵ��� ���
    /// </summary>
    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
