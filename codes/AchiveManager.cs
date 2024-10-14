using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ĳ���� �ر� ���� ����
/// </summary>
public class AchiveManager : MonoBehaviour
{

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait; // ������ �ʴ� �ð�

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);   // �Ź� �θ��� �ʰ� �ѹ��� �����ϱ� ���� Awake���� ������ �ִ´�
        // ������ �ѹ��� ������� �ʾ����� ��� ����Ǵ� �ڵ� 
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    void Init()
    {
        // ������ ���� ����� �����ϴ� ����Ƽ ���� Ŭ���� 
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
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    // ������ �߻��ƴ��� Ȯ��
            lockCharacter[index].SetActive(!isUnlock);  // �رݽ� ��Ȱ��ȭ
            unlockCharacter[index].SetActive(isUnlock);  // �رݽ� Ȱ��ȭ
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
    /// ���� �޼� ���� üũ
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

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) // ���� �޼��� ������ ������ �������� ���� �����̶��
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            // ���� �޼� ������ �����ش�
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            
            StartCoroutine(NoticeRoutine());    // 5���� ��Ȱ��ȭ
        }
    }


    /// <summary>
    /// ���� ������ 5�� ��Ÿ���ٰ� �������
    /// </summary>
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); // ���� �˶� ȿ����

        yield return wait;

        uiNotice.SetActive(false);
    }
}
