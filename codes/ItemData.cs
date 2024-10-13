using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������ ��� ��ũ���ͺ� ������ 
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]   // Ŀ���� �޴� ����
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }


    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]  // �ν����Ϳ� �ؽ�Ʈ�� ������ �ֱ� ����
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages; // ������ ������
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;   // ����ü
    public Sprite hand; // �տ� �׷����� ���� ����

}
