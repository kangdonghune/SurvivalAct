using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum SceneType { Ready, Lobby, Game, None}
    public enum ItemType { Shovel, Fork, Sickle, Gun, Glove, Shoe, Heal, None}
    public enum LoadLabel { Prefab, Animation, Data, Audio,UI, Sprite}
    public enum EnemyType { Jombie, LeatherJombie, Demon, Tree, Skull}
    public enum PlayerName { PlayerWarrior, PlayerThief, PlayerSaint, PlayerCaptain, PlayerKnight, PlayerMage}
    public enum EnemyName { EnemyJombie, EnemyLeatherJombie, EnemyDemon, EnemyTree,EnemySkull}
}
