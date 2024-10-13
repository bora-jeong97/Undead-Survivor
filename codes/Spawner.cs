using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// spawnPoint에 몬스터 스폰
/// </summary>
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();  // 모든 transform을 가져온다. 본인 포함
        //Debug.Log($"spawnPoint : {spawnPoint.Length} ");
    }

    private void Update()
    {

        if (!GameManager.instance.isLive)   // 시간 흐름
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length-1);

        // 시간마다 소환
        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        // 스폰 포인트에서만 생성되게 Spawner 본인을 빼기 위해 0이 아닌 1부터 시작함. 
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// 직렬화(Serialization) 개체를 저장 혹은 전송하기 위해 변환.
// 이 속성을 추가함으로써 inspector에서 직렬화한 클래스를 초기화 할 수 있음.
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}