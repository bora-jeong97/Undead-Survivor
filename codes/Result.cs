using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지거나 승리한 결과 ui 반환
/// </summary>
public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        titles[0].SetActive(true);
    }
    public void Win()
    {
        titles[1].SetActive(true);
    }
}
