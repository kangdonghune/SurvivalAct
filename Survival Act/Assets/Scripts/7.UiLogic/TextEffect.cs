using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    private Text _text;
    private float _fixedTime = 1 / 60f;// 현재 게임 목표 프레임으로 1초를 나눈다.
    private Color _color; // 현재 타이틀의 컬러
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

    //텍스트 효과는 게임 멈춰있음과 별개로 진행되야 하기에 1초를 목표 프레임으로 나눠서 사용 사용
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
