using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class ResourceManager
{
	Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

	public T Load<T>(string key) where T : Object
	{
		if (_resources.TryGetValue(key, out Object resource))
			return resource as T;

		return null;
	}

	public GameObject Instantiate(string key, Transform parent = null )
	{
		GameObject prefab = Load<GameObject>($"{key}");
		if (prefab == null)
		{
			Debug.Log($"Failed to load prefab : {key}");
			return null;
		}

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;
		return go;
	}

	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

        Object.Destroy(go);
	}

	//주의 addressable은 Async라는 이름에서 알 수 있듯이 비동기로 진행된다.
	//현 수준에선 매 순간 비동기를 관리하기보단 게임 시작 또는 다음 씬을 가기 전 리소스를 전부 가져 온 뒤 진행하는 식으로 하는 게 문제를 막을 수 있다.
	#region 어드레서블
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		//이미 로드한 데이터면 바로 콜백
		if (_resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		// 리소스 비동기 로딩 시작.
		var asyncOperation = Addressables.LoadAssetAsync<T>(key);
		asyncOperation.Completed += (op) => //op is operation
		{
			_resources.Add(key, op.Result); //로드한 데이터를 딕셔너리에 추가
			callback?.Invoke(op.Result);
		};
	}


	//해당 라벨 전부 로드 끝나면 true 반환
	public void LoadAllAsync<T>(string label, Action<bool> callback) where T : UnityEngine.Object
	{
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			foreach (var result in op.Result)
			{
				LoadAsync<T>(result.PrimaryKey, (obj) =>
				{
					loadCount++;
					if(loadCount == totalCount) // 전부 로드했다면
						callback?.Invoke(true);
				});
			}
		};
	}

	#endregion
}
