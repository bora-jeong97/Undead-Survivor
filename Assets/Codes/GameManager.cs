using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 외부에서 자주 접근하는 클래스들을 들고있다
/// </summary>느
public class GameManager : MonoBehaviour
{
    public static GameManager instance;// 일단은 장면이 1개이기 때문에 싱글톤을 쓰지않고 메모리(static 정적지정)에 올림.

    public float gameTime;
    public float maxGameTime = 2 * 10f; // 20초

    public PoolManager pool;
    public Player player;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        // 시간마다 소환
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

}
