using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRushCtrl : EnemyController
{
    //생성 시 플레이어 쪽으로 돌진 방향을 정하고 유도가 아니라 해당 방향으로 쭉 돌진
    private Vector2 _rushDir;

    public override void Init(SpawnData data, int animNum)
    {
        base.Init(data, animNum);
        _rushDir = (Managers.Game.Player.transform.position - transform.position).normalized;
        _sprite.flipX = Managers.Game.Player.transform.position.x <= transform.position.x;
    }

    private void FixedUpdate()
    {
        if (Managers.Game.IsLive == false)
            return;

        if (Managers.Game.Player == null || Managers.Game.Hp <= 0)
            return;

        if (isLive == false)
            return;
        _rigid.MovePosition(_rigid.position + _rushDir * Speed * Time.fixedDeltaTime);
        _rigid.velocity = Vector2.zero;
    }
}
