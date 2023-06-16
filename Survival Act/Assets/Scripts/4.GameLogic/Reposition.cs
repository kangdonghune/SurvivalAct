using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    public Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        if (collision.gameObject.activeSelf == false) //플레이어 사망으로 인해 Area 비활성화 시
            return;

        Vector3 playerPos = Managers.Game.Player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = playerPos.x - myPos.x;
        float diffY = playerPos.y - myPos.y;
        float dirX = diffX < 0 ? -1 : 1;
        float dirY = diffY < 0 ? -1 : 1;

        diffX = Mathf.Abs(diffX);
        diffY = Mathf.Abs(diffY);

        switch(transform.tag)
        {
            case "Ground":
                if (Mathf.Abs(diffX - diffY) < 0.01f)
                {
                    transform.Translate(dirX * 40, dirY * 40, 0);
                }
                else if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dir = playerPos - myPos;
                    transform.Translate(dir.normalized * 25 + new Vector3(Random.Range(-3f,3f), Random.Range(-3f, 3f),0));
                }
                break;
            
        }



    }
}
