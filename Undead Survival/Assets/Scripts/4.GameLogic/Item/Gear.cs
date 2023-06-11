using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using static Define;
using System;

public class Gear : MonoBehaviour
{
    public ItemData Data;
    public ItemType Type;
    private float _rate;

    public void Init(ItemData data)
    {
        name = "Gear: " + data.name;
        transform.parent = Managers.Game.Player.transform;
        transform.localPosition = Vector3.zero;
        Type = data.E_Type;
        _rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        _rate = rate;
        ApplyGear();
    }

    private void ApplyGear()
    {
        switch(Type)
        {
            case ItemType.Glove:
                RateUp();
                break;
            case ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            switch(weapon.Type)
            {
                case ItemType.Shovel:
                    weapon.Speed = weapon.Speed + (weapon.Speed * _rate) * Charicter.Speed;
                    break;
                default:
                    weapon.Speed = weapon.Speed * (1f - _rate) * Charicter.Speed;
                    break;
            }
        }
    }

    //이동 속도 관련
    private void SpeedUp()
    {
        float speed = Managers.Game.Player.Speed;
        Managers.Game.Player.Speed = speed + speed * _rate;
    }
}
