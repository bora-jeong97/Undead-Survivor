using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 내내 메모리에 항상 들어고있어 외부에서 쉽게 접근할 수 있다
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;// 일단은 장면이 1개이기 때문에 싱글톤을 쓰지않고 메모리(static 정적지정)에 올림.
    [Header("# Game Control")]
    public bool isLive; // 게임 시간 흐름 여부
    public float gameTime;
    public float maxGameTime = 2 * 10f; // 20초
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 20, 40, 70, 100, 150, 200, 300 };
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;


    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;   // 프레임 수치 지정
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth; // 초기 체력은 최대 체력으로 표시

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);    // 임시 스크립트 (첫번째 캐릭터 선택)
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // 캐릭터 선택 효과음 재생
    }

    /// <summary>
    /// 게임오버시 ui 호출
    /// </summary>
    public void GameOver()
    {
        // 묘비 모션을 주기위해 약간의 딜레이 
        StartCoroutine(GameOverRoutine());
    }

    /// <summary>
    /// 게임승리시 ui 호출
    /// </summary>
    public void GameVictory()
    {
        // 묘비 모션을 주기위해 약간의 딜레이 
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose); // 패배 효과음
    }


    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // 승리 효과음
    }


    /// <summary>
    /// 게임오버 후 돌아가기
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 게임종료
    /// </summary>
    public void GameQuit()
    {
        Application.Quit(); // 애플리케이션 종료 빌드시에만 작용
    }

    private void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        // 시간마다 소환
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();  // 게임승리
        }
    }


    /// <summary>
    /// 경험치 추가
    /// </summary>
    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    /// <summary>
    /// 시간을 멈춘다
    /// </summary>
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // 유니티의 시간 속도(배율)
        uiJoy.localScale = Vector3.zero;    
    }


    /// <summary>
    /// 시간을 재개한다
    /// </summary>
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
