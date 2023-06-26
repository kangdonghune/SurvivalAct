using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class EnemyController : MonoBehaviour
{
    public float Speed;
    public float Hp;
    public float MaxHp;
    public static Transform Target = null;
    public Animator Anim;
    public bool IsLive = false;

    protected Rigidbody2D _rigid;
    protected Collider2D _coll;
    protected SpriteRenderer _sprite;
    protected WaitForFixedUpdate _wait;


    protected  void OnEnable()
    {
        IsLive = true;
        Hp = MaxHp;
        _coll.enabled = true;
        _rigid.simulated = true;
        _sprite.sortingOrder++;
        Anim.SetBool("Dead", false);
        Managers.Enemy.EnemyLst.Add(this);
    }

    protected void OnDisable()
    {
        Managers.Enemy.EnemyLst.Remove(this);
    }


    public virtual void Init(SpawnData data, int animNum)
    {
        //만약에라도 보관하고 있는 애니 컨트롤러보다 길이가 크다면 아웃 오브 인덱스 방지
        Anim.runtimeAnimatorController = Managers.Game.EnemyAniCtrl[animNum % Managers.Game.EnemyAniCtrl.Length];
        if(Target == null)
            Target = Managers.Game.Player.GetComponent<Transform>();
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

    public virtual void FixedUpdateByManager()
    {
        
    }

    public virtual void Move(Vector3 position)
    {

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
        IsLive = false;
        _coll.enabled = false;
        _rigid.simulated = false;
        _sprite.sortingOrder--;
        Anim.SetBool("Dead", true);
    }

    //몬스터 종류에 따라 죽을 때 경험치 등을 안 남길 수 있다.(Tree Or Skull)
    protected virtual void Dead()
    {
        gameObject.SetActive(false); 
    }
}
