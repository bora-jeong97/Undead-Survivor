using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ���� �޸𸮿� �׻� �����־� �ܺο��� ���� ������ �� �ִ�
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;// �ϴ��� ����� 1���̱� ������ �̱����� �����ʰ� �޸�(static ��������)�� �ø�.
    [Header("# Game Control")]
    public bool isLive; // ���� �ð� �帧 ����
    public float gameTime;
    public float maxGameTime = 2 * 10f; // 20��
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
        Application.targetFrameRate = 60;   // ������ ��ġ ����
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth; // �ʱ� ü���� �ִ� ü������ ǥ��

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);    // �ӽ� ��ũ��Ʈ (ù��° ĳ���� ����)
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // ĳ���� ���� ȿ���� ���
    }

    /// <summary>
    /// ���ӿ����� ui ȣ��
    /// </summary>
    public void GameOver()
    {
        // ���� ����� �ֱ����� �ణ�� ������ 
        StartCoroutine(GameOverRoutine());
    }

    /// <summary>
    /// ���ӽ¸��� ui ȣ��
    /// </summary>
    public void GameVictory()
    {
        // ���� ����� �ֱ����� �ణ�� ������ 
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose); // �й� ȿ����
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // �¸� ȿ����
    }


    /// <summary>
    /// ���ӿ��� �� ���ư���
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void GameQuit()
    {
        Application.Quit(); // ���ø����̼� ���� ����ÿ��� �ۿ�
    }

    private void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        // �ð����� ��ȯ
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();  // ���ӽ¸�
        }
    }


    /// <summary>
    /// ����ġ �߰�
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
    /// �ð��� �����
    /// </summary>
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // ����Ƽ�� �ð� �ӵ�(����)
        uiJoy.localScale = Vector3.zero;    
    }


    /// <summary>
    /// �ð��� �簳�Ѵ�
    /// </summary>
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
