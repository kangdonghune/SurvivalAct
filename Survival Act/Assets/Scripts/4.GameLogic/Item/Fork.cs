using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fork : Projectile
{
    private float _distance = 6f;
    private float _curDist = 0;
    private Vector2 _dir;
    private float _dirMag;
    private float _speed = 10f;
    private bool _isReturn = false;

    private void OnEnable()
    {
        _curDist = 0; //������� �̵��Ÿ� �ʱ�ȭ
        _isReturn = false;
    }

    public override void Init(float damage, int per, Vector3 dir)
    {
        base.Init(damage, per, dir);
        _dir = dir;
        _dirMag = _dir.magnitude;
    }

    private void FixedUpdate()
    {
        if(_isReturn == false)
        {
            _curDist += _dirMag * _speed * Time.fixedDeltaTime;
            if (_curDist >= _distance)
            {
                _isReturn = true;
            }
        }
        else
        {
            _dir = Managers.Game.Player.transform.position - transform.position;//���ƿ��� ������ �ٽ� �� ������ ����
            gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        }
        _rigid.MovePosition(_rigid.position + _dir * _speed * Time.fixedDeltaTime);
        _rigid.velocity = Vector2.zero;

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player") && _isReturn == true) //�浹�� �ݸ����� �÷��̾� �±� + ���� ��ũ�� ��ȯ ����
            gameObject.SetActive(false);
    }
}
