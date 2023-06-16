using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class AchiveManager
{
    public void Init()
    {
       
        if (PlayerPrefs.HasKey("MyData") == false)
        {
            PlayerPrefs.SetInt("MyData", 1);
            foreach (PlayerName achive in Enum.GetValues(typeof(PlayerName))) //업적 전부 미달성으로 처리
                PlayerPrefs.SetInt(achive.ToString(), 1); // 1 is Lock
            PlayerPrefs.SetInt(PlayerName.PlayerWarrior.ToString(), 0); // 기본 캐릭터만 해금
        }
    }
    public void UpdateAchive()
    {
        foreach (PlayerName achive in Enum.GetValues(typeof(PlayerName)))
        {
            CheckAchive(achive);
        }
    }

    private void CheckAchive(PlayerName achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case PlayerName.PlayerThief:
                isAchive = true; //한 판이라도 플레이하면 자동 해금 되게 끔
                break;
            case PlayerName.PlayerSaint:
                if (Managers.Game.GameTime >= Managers.Game.MaxGameTime) // 1번이라도 클리하면
                    isAchive = true;
                break;
            case PlayerName.PlayerCaptain:
                if (Managers.Game.Kill >= 200) //200킬 이상 시 해금
                    isAchive = true;
                break;
            case PlayerName.PlayerKnight:
                if (Managers.Game.Kill >= 500) //200킬 이상 시 해금
                    isAchive = true;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 1) //해금을 했고 저장된 값은 비해금이라면
        {
            PlayerPrefs.SetInt(achive.ToString(), 0); //해금으로 새로 저장

            GameObject notice = Managers.UI.Get<GameObject>(achive.ToString());
            if(notice != null)
            {
                notice.SetActive(true);
                Managers.Instance.StartCoroutine(CoNoticePop(notice));
            }
        }
    }

    IEnumerator CoNoticePop(GameObject noticeItem)
    {
        GameObject notice = Managers.UI.Get<GameObject>("Notice");
        if (notice == null)
            yield break;

        notice.SetActive(true);
        Managers.Audio.PlaySFX(AudioManager.SFX.LevelUp);
        yield return new WaitForSecondsRealtime(3);
        notice.SetActive(false);
        noticeItem.SetActive(false);
    }
}
