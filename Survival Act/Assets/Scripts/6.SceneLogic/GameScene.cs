using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    private void Start()
    {
        Managers.Game.GameStart(Managers.Game.PlayerId);
    }

    protected override void Init()
    {
        Type = SceneType.Game;
    }

    public override void Clear()
    {
        Managers.Game.Pool = null;
    }
}
