using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class EnemyController : MonoBehaviour
{
    public float Speed;
    public float Hp;
    public float MaxHp;
    public static Rigidbody2D Target = null;
    public Animator Anim;

    protected Rigidbody2D _rigid;
    protected Collider2D _coll;
    protected SpriteRenderer _sprite;
    protected WaitForFixedUpdate _wait;
    protected bool isLive = false;


    protected  void OnEnable()
    {
        isLive = true;
        Hp = MaxHp;
        _coll.enabled = true;
        _rigid.simulated = true;
        _sprite.sortingOrder++;
        Anim.SetBool("Dead", false);
    }

    public virtual void Init(SpawnData data, int animNum)
    {
        //���࿡�� �����ϰ� �ִ� �ִ� ��Ʈ�ѷ����� ���̰� ũ�ٸ� �ƿ� ���� �ε��� ����
        Anim.runtimeAnimatorController = Managers.Game.EnemyAniCtrl[animNum % Managers.Game.EnemyAniCtrl.Length];
        if(Target == null)
            Target = Managers.Game.Player.GetComponent<Rigidbody2D>();
        Speed = data.speed;
        MaxHp = data.hp;
        Hp = MaxHp;
    }

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        _wait = new WaitForFixedUpdate();
    }

    

    protected IEnumerator CoKnockBack()
    {
        yield return _wait; //1 Frame
        Vector3 playerPos = Managers.Game.Player.transform.position;
        Vector3 dir = -1 * (playerPos - transform.position).normalized;
        _rigid.AddForce(dir * 3, ForceMode2D.Impulse); 
    }

    public void ReadyDead()
    {
        isLive = false;
        _coll.enabled = false;
        _rigid.simulated = false;
        _sprite.sortingOrder--;
        Anim.SetBool("Dead", true);
    }

    //���� ������ ���� ���� �� ����ġ ���� �� ���� �� �ִ�.(Tree Or Skull)
    protected virtual void Dead()
    {
        gameObject.SetActive(false); 
    }
}
