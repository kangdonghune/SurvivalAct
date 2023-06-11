using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public Vector2 InputVec { get; private set; }
    public Scanner Scanner;
    //public Hand[] Hands; ���� ���� ������ ���ҽ��� ���� �� ���� ����
 
    protected Rigidbody2D _rigid;
    protected SpriteRenderer _sprite;
    protected Animator _anim;
    protected Spawner _spawn;
    protected float _collectDist = 1f;

    private void OnEnable()
    {
        //���࿡�� �����ϰ� �ִ� �ִ� ��Ʈ�ѷ����� ���̰� ũ�ٸ� �ƿ� ���� �ε��� ����
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

    //���� ������ fixedUpdate���� ����
    private void FixedUpdate()
    {
        if (Managers.Game.Hp <= 0) //������ �������� �ʰ� ����
            return;
        if (Managers.Game.IsLive == false)
            return;
        //��ġ�̵�
        _rigid.MovePosition(_rigid.position + InputVec * Time.fixedDeltaTime * Speed);
    }

    private void OnMove(InputValue value)
    {
        //nomalize �� ���� �Ѱ��ش�.
        InputVec = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        if (Managers.Game.IsLive == false)
            return;

        foreach(GameObject dropItem in Managers.Game.Grid.GetObjects(transform.position, _collectDist))
        {
            Managers.Game.GetExp();// �ٴڿ� ����� ����ġ ������ ���� �ÿ��� ����ġ ����.
            dropItem.SetActive(false);// ��� ������ ��Ȱ��ȭ
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
                transform.GetChild(idx).gameObject.SetActive(false); //�÷��̾� ���� ������Ʈ ��Ȱ��ȭ
            _anim.SetTrigger("Dead");
            Managers.Game.GameOver();
        }
    }
}
