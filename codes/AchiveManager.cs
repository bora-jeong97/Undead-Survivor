using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 캐릭터 해금 업적 관리
/// </summary>
public class AchiveManager : MonoBehaviour
{

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;


    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        // 게임이 한번도 실행되지 않았으라 경우 실행되는 코드 
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    /// <summary>
    /// 최초 초기화
    /// </summary>
    void Init()
    {
        // 간단한 저장 기능을 제공하는 유니티 제공 클래스 
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);
        }
    }


    void UnlockCharacter()
    {
        for(int index = 0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    // 업적이 발생됐는지 확인
            lockCharacter[index].SetActive(!isUnlock);  // 해금시 비활성화
            unlockCharacter[index].SetActive(isUnlock);  // 해금시 활성화
        }
    }

    private void Start()
    {
        UnlockCharacter();
    }

    private void Update()
    {
        
    }
}
