using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public int per;

    protected Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public virtual void Init(float damage, int per, Vector3 dir)
    {
        this.Damage = damage;
        this.per = per;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (per == -100) // -100 은 관통 무제한
            return;

        if (!collision.CompareTag("Enemy"))
            return;

        if (--per <= 0)
        {
     
            _rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        gameObject.SetActive(false);
    }
}
