using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool IsLeft;
    public SpriteRenderer Sprite;

    private SpriteRenderer _playerSprite;

    Vector3 rigthtPos = new Vector3(0.325f, -0.125f, 0);
    Vector3 rigthtPosReverse = new Vector3(-0.325f, -0.125f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);



    private void Awake()
    {
        _playerSprite = GetComponentsInParent<SpriteRenderer>()[1];//내 손에도 스프라이트가 있다.
    }

    private void LateUpdate()
    {
        bool isReverse = _playerSprite.flipX;
        if(IsLeft) // melee weapon
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            Sprite.flipY = isReverse;
            Sprite.sortingOrder = isReverse ? 4 : 6;
        }

        else if (Managers.Game.Player.Scanner.NearstTarget != null)
        {
            Vector3 targetPos = Managers.Game.Player.Scanner.NearstTarget.position;
            Vector3 dir = targetPos - transform.position;
            transform.localRotation = Quaternion.FromToRotation(Vector3.right, dir);

            bool isRotA = transform.localRotation.eulerAngles.z > 90 && transform.localRotation.eulerAngles.z < 270;
            bool isRotB = transform.localRotation.eulerAngles.z < -90 && transform.localRotation.eulerAngles.z > -270;
            Sprite.flipY = isRotA || isRotB;
            Sprite.sortingOrder = isReverse ? 6 : 4;
        }

        else // Range weapon
        {
            transform.localPosition = isReverse ? rigthtPosReverse : rigthtPos;
            Sprite.flipX = isReverse;
            Sprite.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
