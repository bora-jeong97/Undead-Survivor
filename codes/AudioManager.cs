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
    AudioHighPassFilter bgmEffect;  // Listener Effect계열. 음향조절용, 보다 프리퀀시가 낮은 시그널은 차단한다.

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;    // 다량의 효과음을 낼 수 있도록 채널 개수 변수 선언
    AudioSource[] sfxPlayers;
    int channelIndex;   // 맨 마지막에 플레이 했던 index

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win}

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // object의 audioSource 컴포넌트로 붙여 넣는다
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); 

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels]; // 배열 초기화

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;                     
            sfxPlayers[index].bypassListenerEffects = true; // 배경음만 줄이고 효과음은 그대로 두기 위해 AudioManager > Audio Source - Bypass Listener Effects를 true로 체크해서 무시하도록 한다
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    /// <summary>
    /// 배경음 재생
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
    /// 배경음 조절 함수 
    /// </summary>
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    /// <summary>
    /// 오디오 소스에 오디오 클립 효과음을 순차적으로 저장하고 재생한다 
    /// </summary>
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length; // 마지막 인덱스부터 실행하되 총 길이를 넘지 않도록 나눈다

            if (sfxPlayers[loopIndex].isPlaying)    // 만약 이미 플레이 중이라면
                continue;   // 반복문 도중 다음 루프로 건너팀

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)  // 두개의 효과음을 랜덤으로 내고 싶을 시 하나의 열거형으로 인덱스를 랜덤 사용
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            // 오디오소스의 클립을 변경하고 Play 함수 호출
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }


    }
}
