using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaserCtrl : EnemyController
{
    public override void FixedUpdateByManager()
    {
        //if (Managers.Game.IsLive == false)
        //    return;

        //if (Managers.Game.Player == null || Managers.Game.Hp <= 0)
        //    return;


        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dir = (Target.position - _rigid.position).normalized;
        _rigid.MovePosition(_rigid.position + dir * Speed * Time.fixedDeltaTime);
        _rigid.velocity = Vector2.zero;
        _sprite.flipX = Target.position.x <= _rigid.position.x;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !IsLive) // 중복 충돌 방지
            return;
        Hp -= collision.GetComponent<Projectile>().Damage;

        if (Hp > 0)
        {
            Anim.SetTrigger("Hit");
            StartCoroutine(CoKnockBack());
            Managers.Audio.PlaySFX(AudioManager.SFX.Hit);
        }
        else
        {
            ReadyDead();
            Managers.Game.Kill++;
            Managers.Game.GetExp();
            Managers.Game.Pool.Get("Exp").transform.position = transform.position; //사망 시 내 위치에 Exp 남기기
            if (Managers.Game.IsLive)
                Managers.Audio.PlaySFX(AudioManager.SFX.Dead);
        }
    }
}
