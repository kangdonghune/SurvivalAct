using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using static Define;

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
	public Dictionary<string, ItemData> ItemDic { get; private set; } = new Dictionary<string, ItemData>();
	public Dictionary<string, PoolData> PoolDic { get; private set; } = new Dictionary<string, PoolData>();
	public Dictionary<string, AudioData> AudioDic { get; private set; } = new Dictionary<string, AudioData>();
	public Dictionary<int, CharicterData> CharicterDic { get; private set; } = new Dictionary<int, CharicterData>();
	public Dictionary<string, InGameData> InGameDic { get; private set; } = new Dictionary<string, InGameData>();
	public Dictionary<EnemyType, SpawnData> SpawnDataDic { get; private set; } = new Dictionary<EnemyType, SpawnData>();

	public void Init()
	{
		ItemDic = LoadJson<Data.ItemDataLoader, string, ItemData>("ItemData").MakeDict();
		PoolDic = LoadJson<Data.PoolDataLoader, string, PoolData>("PoolData").MakeDict();
		AudioDic = LoadJson<Data.AudioDataLoader, string, AudioData>("AudioData").MakeDict();
		CharicterDic = LoadJson<Data.CharicterDataLoader, int, CharicterData>("CharicterData").MakeDict();
		InGameDic = LoadJson<Data.InGameDataLoader, string, InGameData>("InGameData").MakeDict();
		SpawnDataDic = LoadJson<Data.SpawnDataLoader,EnemyType, SpawnData>("SpawnData").MakeDict();
	}

	Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
		return JsonUtility.FromJson<Loader>(textAsset.text);
	}

}
