using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    public override void Init()
    {
        Get<LevelUp>("LevelUp").Init();
        Managers.UI.Get<Button>("ReTry").onClick.AddListener(() =>Managers.Game.GameRetry());
    }
}
