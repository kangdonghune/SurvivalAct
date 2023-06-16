using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class ChariterUI : MonoBehaviour
{
    public int CharicterID;


    //로비에서 캐릭터 선택 시 변경
    public void UIUpdate(CharicterData data, bool isLock, bool isGroup = false)
    {
        Image icon = transform.GetChild(0).GetComponent<Image>();
        Text nameText = transform.GetChild(1).GetComponent<Text>();
        Text descText = transform.GetChild(2).GetComponent<Text>();

        CharicterID = data.id;
        icon.color = new Color(1, 1, 1, 1);
        icon.sprite = Managers.Resource.Load<Sprite>(data.icon);
        icon.name = "Icon: " + data.name;
        if (isLock)
        {
            icon.color = new Color(0f, 0f, 0f, 1f); //icon 가리기
            nameText.text = "???";
            descText.text = data.locktext; //해금 조건으로 텍스트 변경
        }
        else
        {
            nameText.text = data.name;
            descText.text = data.buff;
        }

        if (isGroup)
        {
            transform.GetChild(0).GetComponent<Image>().name = "Icon: " + data.name;
            nameText.name = "Name: " + data.name;
            descText.GetComponent<Text>().name = "Buff: " + data.name;
            if (isLock == false)
                gameObject.GetComponent<Image>().color = new Color(data.color[0]/255f, data.color[1] / 255f, data.color[2] / 255f, 255);
            else
            {
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                descText.text = "잠김";
                descText.color = new Color(1f, 0f, 0f, 255);
            }
        }
    }
}
