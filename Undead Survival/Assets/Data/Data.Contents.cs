using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{
    #region ������ ����
    [Serializable]
    public class ItemData
    {
        public string name;
        public string desc;
        public string type;
        public float baseDamage; //0���� ������
        public int baseCount;  //0���� ���� �Ǵ� ���� ��
        public float baseSpeed; //0���� ���� ���� �ӵ�
        public float[] damages; //���� �� �����ϴ� ������ ��
        public int[] counts; //���� �� �����ϴ� 
        public string hand; //�տ� �� ���� ��������Ʈ �̸�
        public string projectile; // ����ü �̸�
        public string icon; //������ ���� â�� ��� �� ������

        //
        public ItemType E_Type; // string to enum���� ��ȯ�Ͽ� ����
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

    #region Ǯ�� ��� ����
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

    #region ����� ��� ����
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

    #region InGameData //���� �� ����ġ. �ð� �� �� ����(ex: 0~30 0 ����, 30~1�� 1���� ��),�ð� �� Ư�� ���� ���� ���� ���
    [Serializable]
    public class InGameData
    {
        public string type; //EXp, Monseter�� ���� Ÿ��, Ư�� ���� ���� �ð�
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
        //���� ��ư ���� �� ���
        public string icon; 
        public string name;
        public string buff; // �÷��̾� �� ���� �ؽ�Ʈ
        public string locktext; //�ر� ����
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
