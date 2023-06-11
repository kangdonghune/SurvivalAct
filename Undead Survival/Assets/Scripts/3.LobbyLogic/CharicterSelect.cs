using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System;
using static Define;
public class CharicterSelect : MonoBehaviour
{
    public void CreateCharicterUI()
    {
        Transform parent = Managers.UI.Get<Transform>("CharitorGroup");

        int[] unLocks = new int[Enum.GetValues(typeof(PlayerName)).Length]; //unlock value 1 = lock, 0 = unlock
        for(int idx= 0; idx <unLocks.Length;idx++)
        {
            unLocks[idx] = PlayerPrefs.GetInt(((PlayerName)idx).ToString());
        }

        for(int idx = 0; idx < unLocks.Length; idx++)
        {
            //layout 컴퍼넌트에 자식으로 넣어줄 땐 생성 단계에서 부모로 해줘야지 문제가 없고 나중에 setParent 등으로 연결하면 문제가 발생한다.
            bool isLock = Convert.ToBoolean(unLocks[idx]);
            int curID = 0;
            ChariterUI chariterUI = Managers.Resource.Instantiate("Charicter", parent).GetComponent<ChariterUI>();
            CharicterData data = Managers.Data.CharicterDic[idx];
            chariterUI.UIUpdate(data, isLock, true);
            chariterUI.gameObject.name = "캐릭터: " + data.name;
            if (isLock == true)
                curID = -1;
            else
                curID = data.id;
            chariterUI.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Managers.Game.PlayerId = curID;
                    Managers.UI.Get<ChariterUI>("SelectChariter").UIUpdate(data, isLock, false);
                    Managers.Audio.PlaySFX(AudioManager.SFX.Select);
                });
        }
        Managers.UI.UpdateDict();//새로운 ui가 추가되었으니 갱신
    }
}
