using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = Managers.Game.exp;
                float maxExp = Managers.Game.nextExp[Mathf.Min(Managers.Game.Level, Managers.Game.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("LV.{0:F0}", Managers.Game.Level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", Managers.Game.Kill);
                break;
            case InfoType.Time:
                float remainTime = Managers.Game.MaxGameTime - Managers.Game.GameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHP = Managers.Game.Hp;
                float maxHP = Managers.Game.MaxHp;
                mySlider.value = curHP / maxHP;
                break;
        }
    }
}
