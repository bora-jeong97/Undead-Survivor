using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ĳ���� ���� Ư¡ ����
/// </summary>
public class Character : MonoBehaviour
{
    /// <summary>
    /// �ҳ�� Ư�� �̵��ӵ� 10% ����
    /// </summary>
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    /// <summary>
    /// ������� Ư�� ���� ���� �ӵ� 10% ���� (�������� - ����)
    /// </summary>
    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }


    /// <summary>
    /// ������� Ư�� ���� ���� �ӵ� 10% ���� (���Ÿ� - ��)
    /// </summary>
    public static float WeaponRate 
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    
    /// <summary>
    /// ���ڳ�� Ư�� ������ 20% ����
    /// </summary>
    public static float Damage
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    /// <summary>
    /// ���� Ư�� ����ü ����
    /// </summary>
    public static int Count
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}
