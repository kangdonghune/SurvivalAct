using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using static Define;
using System;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public string Name;
    public float Dagmage; 
    public int Count; //투사체 개수 Or 관통 대상 수
    public float Speed;//공격 쿨타임 OR 공격 물체 회전 속도
    public ItemType Type;
    public string projectile;

    private float _timer = 0;
    private PlayerController _player;

    private void Awake()
    {
        _player = Managers.Game.Player;
    }

    private void Update()
    {
        if (Managers.Game.IsLive == false)
            return;

        switch (Type)
        {
            case ItemType.Shovel:
                transform.Rotate(Vector3.back * Speed * Time.deltaTime);
                break;
            case ItemType.Fork:
                _timer += Time.deltaTime;
                if (_timer > Speed)
                {
                    _timer = 0;
                    for (int cnt = 0; cnt < Count; cnt++) //투척해야하는 수만큼 반복
                        ThrowFork();
                }
                break;
            case ItemType.Sickle:
                _timer += Time.deltaTime;
                if (_timer > Speed)
                {
                    _timer = 0;
                    for(int cnt = 0; cnt < Count; cnt++) //투척해야하는 수만큼 반복
                        ThrowSickle();
                }
                break;
            case ItemType.Gun:
                _timer += Time.deltaTime;
                if(_timer > Speed)
                {
                    _timer = 0;
                    Fire();
                }
                break;
            default:
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.Dagmage = damage;
        this.Count += count;

        if (Type == ItemType.Shovel)
            CreateShovel();

        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //basic
        name = "Weapon: " + data.name;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;

        Type = data.E_Type;
        Dagmage = data.baseDamage * Charicter.Damage;
        Count = data.baseCount + Charicter.Count;
        Speed = data.baseSpeed;
        projectile = data.projectile;
        switch (data.name)
        {
    
            case "삽":
                Speed *= Charicter.Speed; // 회전 속도
                CreateShovel();
                break;
            case "대형 포크":
                Speed *= Charicter.WeaponRate;// 연사 속도
                break;
            case "낫":
                Speed *= Charicter.WeaponRate;// 연사 속도
                break;
            case "엽총":
                Speed *= Charicter.WeaponRate;// 연사 속도
                break;
            default:
                break;
        }

        //나와 내 모든 자식한테 해당 함수를 호출하도록 명령
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void CreateShovel()
    {
        for(int idx = 0; idx < Count; idx++)
        {
            Transform bullet;
            if (idx < transform.childCount)
            {
                bullet = transform.GetChild(idx);
            }
            else
            {
                bullet = Managers.Game.Pool.Get(projectile).transform;
            }
            bullet.parent = transform;
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            Vector3 rotVec = (Vector3.forward * 360 * idx) / Count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Projectile>().Init(Dagmage, -100, Vector2.zero); //-100 is infinity Per;
        }
    }

    void Fire()
    {
        if (_player.Scanner.NearstTarget == null)
            return;

        Transform bullet = Managers.Game.Pool.Get(projectile).transform;
        Vector3 dir = (_player.Scanner.NearstTarget.position - transform.position).normalized;
        bullet.transform.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(Dagmage, Count, dir);
        //Fire의 Count는 관통 수
        Managers.Audio.PlaySFX(AudioManager.SFX.Range);
    }

    void ThrowFork()
    {
        if (_player.Scanner.NearstTarget == null)
            return;

        Transform bullet = Managers.Game.Pool.Get(projectile).transform;
        int random = Random.Range(0, _player.Scanner.Targets.Length); // 스캔한 적 목록 중 한 곳을 타겟으로 지정
        Vector3 dir = (_player.Scanner.Targets[random].transform.position - transform.position).normalized;
        bullet.transform.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Fork>().Init(Dagmage, -100, dir);
    }

    void ThrowSickle()
    {
        if (_player.Scanner.NearstTarget == null)
            return;
        Transform fork = Managers.Game.Pool.Get(projectile).transform;
        float random = Random.Range(-45f, 45f);
        Vector3 dir = new Vector3(Mathf.Sin(random * Mathf.Deg2Rad), Mathf.Cos(random * Mathf.Deg2Rad), 0);
        fork.transform.position = transform.position; 
        fork.GetComponent<Sickle>().Init(Dagmage, Count, dir);
    }
}
