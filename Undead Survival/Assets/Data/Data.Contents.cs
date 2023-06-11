using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{
    #region 아이템 정보
    [Serializable]
    public class ItemData
    {
        public string name;
        public string desc;
        public string type;
        public float baseDamage; //0레벨 데미지
        public int baseCount;  //0레벨 관통 또는 무기 수
        public float baseSpeed; //0레벨 기준 공격 속도
        public float[] damages; //레벨 별 증가하는 데미지 량
        public int[] counts; //레벨 별 증가하는 
        public string hand; //손에 쥘 무기 스프라이트 이름
        public string projectile; // 투사체 이름
        public string icon; //아이템 선택 창에 띄어 줄 아이콘

        //
        public ItemType E_Type; // string to enum으로 변환하여 보관
        public Sprite Hand;
        public GameObject Projectile;
        public Sprite Icon;

        public void Init()
        {
            E_Type = (ItemType)Enum.Parse(typeof(ItemType), type);
            if (!String.IsNullOrEmpty(hand))
                Hand = Managers.Resource.Load<Sprite>(hand);
            if (!String.IsNullOrEmpty(icon))
                Icon = Managers.Resource.Load<Sprite>(icon);
        }
    }
    [Serializable]
    public class ItemDataLoader : ILoader<string, ItemData>
    {
        public List<ItemData> stats = new List<ItemData>();

        public Dictionary<string, ItemData> MakeDict()
        {
            Dictionary<string, ItemData> dict = new Dictionary<string, ItemData>();
            foreach (ItemData stat in stats)
            {
                stat.Init();
                dict.Add(stat.name, stat);
            }
            return dict;
        }
    }
    #endregion

    #region 풀링 목록 정보
    [Serializable]
    public class PoolData
    {
        public string type;
        public string[] names;
    }

    [Serializable]
    public class PoolDataLoader : ILoader<string, PoolData>
    {
        public List<PoolData> datas = new List<PoolData>();

        public Dictionary<string, PoolData> MakeDict()
        {
            Dictionary<string, PoolData> dict = new Dictionary<string, PoolData>();
            foreach (PoolData data in datas)
                dict.Add(data.type, data);
            return dict;
        }
    }
    #endregion

    #region 오디오 목록 정보
    [Serializable]
    public class AudioData
    {
        public string type;
        public string[] sounds;
    }

    [Serializable]
    public class AudioDataLoader : ILoader<string, AudioData>
    {
        public List<AudioData> datas = new List<AudioData>();

        public Dictionary<string, AudioData> MakeDict()
        {
            Dictionary<string, AudioData> dict = new Dictionary<string, AudioData>();
            foreach (AudioData data in datas)
                dict.Add(data.type, data);
            return dict;
        }
    }
    #endregion

    #region InGameData //레발 당 경험치. 시간 당 맵 레벨(ex: 0~30 0 레벨, 30~1분 1레벨 등),시간 당 특정 패턴 등장 여부 등등
    [Serializable]
    public class InGameData
    {
        public string type; //EXp, Monseter별 등장 타임, 특정 패턴 등장 시간
        public float[] value;
    }
    [Serializable]
    public class InGameDataLoader : ILoader<string, InGameData>
    {
        public List<InGameData> datas = new List<InGameData>();

        public Dictionary<string, InGameData> MakeDict()
        {
            Dictionary<string, InGameData> dict = new Dictionary<string, InGameData>();
            foreach (InGameData data in datas)
                dict.Add(data.type, data);
            return dict;
        }
    }
    #endregion


    #region CharicterData
    [Serializable]
    public class CharicterData
    {
        public int id;
        //선택 버튼 만들 때 사용
        public string icon; 
        public string name;
        public string buff; // 플레이어 별 버프 텍스트
        public string locktext; //해금 조건
        public float[] color;
    }

    [Serializable]
    public class CharicterDataLoader : ILoader<int, CharicterData>
    {
        public List<CharicterData> datas = new List<CharicterData>();

        public Dictionary<int, CharicterData> MakeDict()
        {
            Dictionary<int, CharicterData> dict = new Dictionary<int, CharicterData>();
            foreach (CharicterData data in datas)
                dict.Add(data.id, data);
            return dict;
        }
    }
    #endregion


    #region EnemySpawnData
    [Serializable]
    public class SpawnData
    {
        public string name;
        public int hp;
        public float speed;
        public EnemyType Type;
    }

    [Serializable]
    public class SpawnDataLoader : ILoader<EnemyType, SpawnData>
    {
        public List<SpawnData> datas = new List<SpawnData>();

        public Dictionary<EnemyType, SpawnData> MakeDict()
        {
            Dictionary<EnemyType, SpawnData> dict = new Dictionary<EnemyType, SpawnData>();
            foreach (SpawnData data in datas)
            {
                data.Type = (EnemyType)Enum.Parse(typeof(EnemyType), data.name);
                dict.Add(data.Type, data);
            }
            return dict;
        }
    }
    #endregion


}
