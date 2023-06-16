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
        Get<Text>("StartButtonText").text = "게임 시작";
        Get<Button>("StartButton").enabled = true; //만약에라도 버튼이 비활성화 되었다면 활성화
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
