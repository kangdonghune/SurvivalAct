using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    private Text _text;
    private float _fixedTime = 1 / 60f;// ���� ���� ��ǥ ���������� 1�ʸ� ������.
    private Color _color; // ���� Ÿ��Ʋ�� �÷�
    private float _speed = 1.2f;
    private float _minAlpha = 80 / 255f;
    private float _maxAlpha = 1f;
    private bool _isIncrease = true;

    void Start()
    {
        _text = GetComponent<Text>();
        _color = _text.color;
    }

    void Update()
    {
        TwinkleNaon();
    }

    //�ؽ�Ʈ ȿ���� ���� ���������� ������ ����Ǿ� �ϱ⿡ 1�ʸ� ��ǥ ���������� ������ ��� ���
    void TwinkleNaon()
    {
        _text.color = _color;
        if (_isIncrease == true)
        {
            _color.a += _speed * _fixedTime;
            _color.r += _speed * _fixedTime;
            if (_color.a >= _maxAlpha)
                _isIncrease = false;
        }
        else
        {
            _color.a -= _speed * _fixedTime;
            _color.r -= _speed * _fixedTime;
            if (_color.a <= _minAlpha)
                _isIncrease = true;
        }
    }
}
