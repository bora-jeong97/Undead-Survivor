using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비 관리 
/// </summary>
public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;  // 타입
    public float rate;  // 수치

    /// <summary>
    /// id별 각각 초깃값
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
    /// 장갑 아이템 : 무기의 속도를 올리는 함수
    /// </summary>
    void RateUp()
    {
        
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();  // 부모 컴포넌트로 올라가서 모든 weapon가져오기

        // 모든 무기 속도 up
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // 근접무기
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default: // 원거리 등
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);  // 발사 간격
                    break;
            }
        }
    }

    /// <summary>
    /// 장화 아이템 : 플레이어의 속도를 상승
    /// </summary>
    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
