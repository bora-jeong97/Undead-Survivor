using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;  // Listener Effect�迭. ����������, ���� �������ð� ���� �ñ׳��� �����Ѵ�.

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;    // �ٷ��� ȿ������ �� �� �ֵ��� ä�� ���� ���� ����
    AudioSource[] sfxPlayers;
    int channelIndex;   // �� �������� �÷��� �ߴ� index

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win}

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // object�� audioSource ������Ʈ�� �ٿ� �ִ´�
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); 

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels]; // �迭 �ʱ�ȭ

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;                     
            sfxPlayers[index].bypassListenerEffects = true; // ������� ���̰� ȿ������ �״�� �α� ���� AudioManager > Audio Source - Bypass Listener Effects�� true�� üũ�ؼ� �����ϵ��� �Ѵ�
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    /// <summary>
    /// ����� ���
    /// </summary>
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    /// <summary>
    /// ����� ���� �Լ� 
    /// </summary>
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    /// <summary>
    /// ����� �ҽ��� ����� Ŭ�� ȿ������ ���������� �����ϰ� ����Ѵ� 
    /// </summary>
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length; // ������ �ε������� �����ϵ� �� ���̸� ���� �ʵ��� ������

            if (sfxPlayers[loopIndex].isPlaying)    // ���� �̹� �÷��� ���̶��
                continue;   // �ݺ��� ���� ���� ������ �ǳ���

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)  // �ΰ��� ȿ������ �������� ���� ���� �� �ϳ��� ���������� �ε����� ���� ���
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            // ������ҽ��� Ŭ���� �����ϰ� Play �Լ� ȣ��
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }


    }
}
