using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public Vector2 InputVec { get; private set; }
    public Scanner Scanner;
    //public Hand[] Hands; 현재 새로 변경한 리소스와 손이 안 어울려 제거
 
    protected Rigidbody2D _rigid;
    protected SpriteRenderer _sprite;
    protected Animator _anim;
    protected Spawner _spawn;
    protected float _collectDist = 1f;

    private void OnEnable()
    {
        //만약에라도 보관하고 있는 애니 컨트롤러보다 길이가 크다면 아웃 오브 인덱스 방지
        _anim.runtimeAnimatorController = Managers.Game.PlayerAniCtrl[Managers.Game.PlayerId % Managers.Game.PlayerAniCtrl.Length];
        Speed *= Charicter.Speed;
    }

    private void Awake()
    {
        Scanner = GetComponent<Scanner>();
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    //물리 연산은 fixedUpdate에서 관리
    private void FixedUpdate()
    {
        if (Managers.Game.Hp <= 0) //죽으면 움직이지 않게 조정
            return;
        if (Managers.Game.IsLive == false)
            return;
        //위치이동
        _rigid.MovePosition(_rigid.position + InputVec * Time.fixedDeltaTime * Speed);
    }

    private void OnMove(InputValue value)
    {
        //nomalize 된 값을 넘겨준다.
        InputVec = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        if (Managers.Game.IsLive == false)
            return;

        foreach(GameObject dropItem in Managers.Game.Grid.GetObjects(transform.position, _collectDist))
        {
            Managers.Game.GetExp();// 바닥에 드랍된 경험치 아이템 습득 시에도 경험치 증가.
            dropItem.SetActive(false);// 드랍 아이템 비활성화
            Managers.Audio.PlaySFX(AudioManager.SFX.PickUp);
        }
        _anim.SetFloat("Speed", InputVec.magnitude);

        if (InputVec.x != 0)
            _sprite.flipX = InputVec.x < 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!Managers.Game.IsLive || Managers.Game.Hp <= 0)
            return;

        Managers.Game.Hp -= Time.deltaTime * 10;

        if(Managers.Game.Hp <= 0)
        {
            for (int idx = 0; idx < transform.childCount; idx++)
                transform.GetChild(idx).gameObject.SetActive(false); //플레이어 산하 오브젝트 비활성화
            _anim.SetTrigger("Dead");
            Managers.Game.GameOver();
        }
    }
}
