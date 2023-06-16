using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public override void Init(float damage, int per, Vector3 dir)
    {
        base.Init(damage, per, dir);
        _rigid.velocity = dir * 15f;
    }
}
