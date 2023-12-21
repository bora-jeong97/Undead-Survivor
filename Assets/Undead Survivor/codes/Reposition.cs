using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{

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
                break;

        }



    }
}
