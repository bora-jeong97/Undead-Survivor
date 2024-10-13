using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 캐릭터 선택 특징 적용
/// </summary>
public class Character : MonoBehaviour
{
    /// <summary>
    /// 쌀농부 특성 이동속도 10% 증가
    /// </summary>
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    /// <summary>
    /// 보리농부 특성 무기 연사 속도 10% 증가 (근접무기 - 위성)
    /// </summary>
    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }


    /// <summary>
    /// 보리농부 특성 무기 연사 속도 10% 증가 (원거리 - 총)
    /// </summary>
    public static float WeaponRate 
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    
    /// <summary>
    /// 감자농부 특성 데미지 20% 증가
    /// </summary>
    public static float Damage
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    /// <summary>
    /// 콩농부 특성 투사체 증가
    /// </summary>
    public static int Count
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}
