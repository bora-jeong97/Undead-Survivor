using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 정보를 담는 스크립터블 데이터 
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]   // 커스텀 메뉴 생성
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }


    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]  // 인스펙터에 텍스트를 여러줄 넣기 위함
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages; // 레벨별 데미지
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;   // 투사체
    public Sprite hand; // 손에 그려지는 무기 형태

}
