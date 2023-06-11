using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ReadyUI : BaseUI
{
    public override void Init()
    {
        FuncBinding();
        Get<Text>("StartButtonText").text = "게임 시작";
    }

    private void FuncBinding()
    {
        Get<Button>("StartButton")?.onClick.AddListener(() =>
            {
                Managers.SceneEx.LoadScene(SceneType.Lobby);
                Managers.Audio.PlaySFX(AudioManager.SFX.Select);
            });
    }

    
}
