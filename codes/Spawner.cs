using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// spawnPoint�� ���� ����
/// </summary>
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();  // ��� transform�� �����´�. ���� ����
        //Debug.Log($"spawnPoint : {spawnPoint.Length} ");
    }

    private void Update()
    {

        if (!GameManager.instance.isLive)   // �ð� �帧
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length-1);

        // �ð����� ��ȯ
        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        // ���� ����Ʈ������ �����ǰ� Spawner ������ ���� ���� 0�� �ƴ� 1���� ������. 
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// ����ȭ(Serialization) ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ.
// �� �Ӽ��� �߰������ν� inspector���� ����ȭ�� Ŭ������ �ʱ�ȭ �� �� ����.
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}