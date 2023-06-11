using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using static Define;
using System;

public class Item : MonoBehaviour
{
    public int Level = 0;
    public Weapon Weapon;
    public Gear Gear;
    public ItemData Data;
    public ItemType Type;

    private Image _icon;
    private Text _textLevel;
    private Text _textName;
    private Text _textDesc;

    Texture2D texture;

    public void Init(ItemData data)
    {
        Data = data;
        //ui 자식 연결
        _icon = GetComponentsInChildren<Image>(true)[1];
        Text[] texts = GetComponentsInChildren<Text>(true);
        _textLevel = texts[0];
        _textName = texts[1];
        _textDesc = texts[2];

        _icon.sprite = data.Icon;
        _icon.gameObject.name = "icon " + data.name;
        _textName.text = data.name;
        _textName.gameObject.name = "NameText " + data.name;
        _textLevel.gameObject.name = "LevelText " + data.name;
        _textDesc.gameObject.name = "DescText " + data.name;

        Type = data.E_Type;
        gameObject.name = "아이템: " + data.name;
    }

    private void OnEnable()
    {
        _textLevel.text = "Lv. " + Level;
        switch (Type)
        {
            case ItemType.Shovel:
            case ItemType.Fork:
            case ItemType.Sickle:
            case ItemType.Gun:
                _textDesc.text = string.Format(Data.desc, Data.damages[Level] * 100, Data.counts[Level]);
                break;
            case ItemType.Glove:
            case ItemType.Shoe:
                _textDesc.text = string.Format(Data.desc, Data.damages[Level] * 100); //Damage는 ui 상에서 퍼센트로 출력
                break;
            case ItemType.Heal:
                _textDesc.text = string.Format(Data.desc);
                break;
        }
        
    }

    public void OnClick()
    {
        switch(Type)
        {
            case ItemType.Shovel:
            case ItemType.Fork:
            case ItemType.Sickle:
            case ItemType.Gun:
                if (Level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    Weapon = newWeapon.AddComponent<Weapon>();
                    Weapon.Init(Data);
                }
                else
                {
                    float nextDamage = Data.baseDamage;
                    int nextCount = Data.counts[Level];
                    nextDamage += Data.baseDamage * Data.damages[Level];
                    Weapon.LevelUp(nextDamage, nextCount);
                }
                Level++;
                break;
            case ItemType.Glove:
            case ItemType.Shoe:
                if (Level == 0)
                {
                    GameObject newGear = new GameObject();
                    Gear = newGear.AddComponent<Gear>();
                    Gear.Init(Data);
                }
                else
                {
                    float nextRate = Data.damages[Level];
                    Gear.LevelUp(nextRate);
                }
                Level++;
                break;
            case ItemType.Heal:
                Managers.Game.Hp = Managers.Game.MaxHp;
                break; // 음식은 레벨 업 개념이 없기에 LevelUp을 시키지 않는다.
        }
        //이번에 선택된 아이템이 json 상 설정해둔 레벨의 길이와 같다면 최대 레벨. 버튼 클릭 불가 상태로 전환
        if(Level == Data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }

        Managers.UI.Get<LevelUp>("LevelUp").Hide(); //선택을 종료하면 레벨 업 ui를 숨겨주고 게임을 마저 진행
    }
}
