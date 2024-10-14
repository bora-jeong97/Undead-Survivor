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
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait; // 멈추지 않는 시간

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);   // 매번 부르지 않고 한번만 실행하기 위해 Awake에서 변수에 넣는다
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
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    private void Start()
    {
        UnlockCharacter();
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


    private void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    /// <summary>
    /// 업적 달성 여부 체크
    /// </summary>
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) // 업적 달성을 했으며 기존에 해제되지 않은 업적이라면
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            // 업적 달성 공지를 보여준다
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            
            StartCoroutine(NoticeRoutine());    // 5초후 비활성화
        }
    }


    /// <summary>
    /// 업적 공지가 5초 나타났다가 사라진다
    /// </summary>
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); // 공지 알람 효과음

        yield return wait;

        uiNotice.SetActive(false);
    }
}
