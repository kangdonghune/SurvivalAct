using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : Projectile
{
    public override void Init(float damage, int per, Vector3 dir)
    {
        base.Init(damage, per, dir);
        _rigid.AddForce(dir * 12f, ForceMode2D.Impulse);
        _rigid.AddTorque(-dir.x * 45f, ForceMode2D.Impulse);
    }
}
