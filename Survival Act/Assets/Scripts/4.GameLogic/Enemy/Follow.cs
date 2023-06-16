using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        _rect.position = Camera.main.WorldToScreenPoint(Managers.Game.Player.transform.position + Vector3.down * 0.7f);
    }
}
