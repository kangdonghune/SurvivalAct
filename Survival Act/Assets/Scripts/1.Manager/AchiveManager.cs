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
            foreach (PlayerName achive in Enum.GetValues(typeof(PlayerName))) //���� ���� �̴޼����� ó��
                PlayerPrefs.SetInt(achive.ToString(), 1); // 1 is Lock
            PlayerPrefs.SetInt(PlayerName.PlayerWarrior.ToString(), 0); // �⺻ ĳ���͸� �ر�
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
                isAchive = true; //�� ���̶� �÷����ϸ� �ڵ� �ر� �ǰ� ��
                break;
            case PlayerName.PlayerSaint:
                if (Managers.Game.GameTime >= Managers.Game.MaxGameTime) // 1���̶� Ŭ���ϸ�
                    isAchive = true;
                break;
            case PlayerName.PlayerCaptain:
                if (Managers.Game.Kill >= 200) //200ų �̻� �� �ر�
                    isAchive = true;
                break;
            case PlayerName.PlayerKnight:
                if (Managers.Game.Kill >= 500) //200ų �̻� �� �ر�
                    isAchive = true;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 1) //�ر��� �߰� ����� ���� ���ر��̶��
        {
            PlayerPrefs.SetInt(achive.ToString(), 0); //�ر����� ���� ����

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
