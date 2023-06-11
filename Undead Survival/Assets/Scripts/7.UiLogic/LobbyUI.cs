using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class LobbyUI : BaseUI
{
    public override void Init()
    {
        FuncBinding();
        Get<Text>("StartButtonText").text = "���� ����";
        Get<Button>("StartButton").enabled = true; //���࿡�� ��ư�� ��Ȱ��ȭ �Ǿ��ٸ� Ȱ��ȭ
    }

    private void Start()
    {
        Get<CharicterSelect>().CreateCharicterUI();
    }

    private void FuncBinding()
    {

        Get<Button>("StartButton")?.onClick.AddListener(() =>
            {
                if (Managers.Game.PlayerId == -1)
                    return;
                Managers.SceneEx.LoadScene(SceneType.Game);
                Managers.Audio.PlaySFX(AudioManager.SFX.Select);
            }
        );
    }
}
