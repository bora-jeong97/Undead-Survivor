using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        // area를 벗어나면 return
        if (!collision.CompareTag("Area"))
            return;

        // player위치
        Vector3 playerPosition = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float diffx = Mathf.Abs(playerPosition.x - myPos.x); // Abs : 절댓값
        float diffy = Mathf.Abs(playerPosition.y - myPos.y); // Abs : 절댓값


        // player 방향
        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirX * 40); // Translate: 지정된 값만큼 현재 위치에서 이동
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * dirY * 40); // Translate: 지정된 값만큼 현재 위치에서 이동
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0)); // 플레이어의 맞은편에서도 나타나게 함.
                }
                break;

        }



    }
}
