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
            //layout ���۳�Ʈ�� �ڽ����� �־��� �� ���� �ܰ迡�� �θ�� ������� ������ ���� ���߿� setParent ������ �����ϸ� ������ �߻��Ѵ�.
            bool isLock = Convert.ToBoolean(unLocks[idx]);
            int curID = 0;
            ChariterUI chariterUI = Managers.Resource.Instantiate("Charicter", parent).GetComponent<ChariterUI>();
            CharicterData data = Managers.Data.CharicterDic[idx];
            chariterUI.UIUpdate(data, isLock, true);
            chariterUI.gameObject.name = "ĳ����: " + data.name;
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
        Managers.UI.UpdateDict();//���ο� ui�� �߰��Ǿ����� ����
    }
}
