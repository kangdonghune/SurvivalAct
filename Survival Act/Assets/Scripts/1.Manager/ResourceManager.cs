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

	//���� addressable�� Async��� �̸����� �� �� �ֵ��� �񵿱�� ����ȴ�.
	//�� ���ؿ��� �� ���� �񵿱⸦ �����ϱ⺸�� ���� ���� �Ǵ� ���� ���� ���� �� ���ҽ��� ���� ���� �� �� �����ϴ� ������ �ϴ� �� ������ ���� �� �ִ�.
	#region ��巹����
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		//�̹� �ε��� �����͸� �ٷ� �ݹ�
		if (_resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		// ���ҽ� �񵿱� �ε� ����.
		var asyncOperation = Addressables.LoadAssetAsync<T>(key);
		asyncOperation.Completed += (op) => //op is operation
		{
			_resources.Add(key, op.Result); //�ε��� �����͸� ��ųʸ��� �߰�
			callback?.Invoke(op.Result);
		};
	}


	//�ش� �� ���� �ε� ������ true ��ȯ
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
					if(loadCount == totalCount) // ���� �ε��ߴٸ�
						callback?.Invoke(true);
				});
			}
		};
	}

	#endregion
}
