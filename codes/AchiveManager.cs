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

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;


    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

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
            PlayerPrefs.SetInt(achive.ToString(), 1);
        }
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

    private void Start()
    {
        UnlockCharacter();
    }

    private void Update()
    {
        
    }
}
