using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PoolManager : MonoBehaviour
{
    // �������� ������ ����
    public Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();
    // Ǯ ����� �ϴ� ����Ʈ
    private Dictionary<string, List<GameObject>> _poolDict = new Dictionary<string, List<GameObject>>();
    // ������ �� ��Ʈ
    private Dictionary<string, GameObject> _roots = new Dictionary<string, GameObject>();

    private void Awake()
    {
        //json���� �����ص� Ǯ�� ����� ������ ��ųʸ��� ��ȯ
        foreach (string key in Managers.Data.PoolDic.Keys)
        {
            foreach (string name in Managers.Data.PoolDic[key].names)
            {
                _prefabDict.Add(name, Managers.Resource.Load<GameObject>(name));
            }
        }

        foreach (string key in _prefabDict.Keys)
        {
            _poolDict.Add(key, new List<GameObject>());
            GameObject root = new GameObject($"@{key}");
            root.transform.parent = transform;
            _roots.Add(key, root);
        }
    }

    private void Start()
    {
        CreatePool(100);
    }

    //���� ���� ���� �� ���� �� �����ϴ� ���� ��� ��ȯ ���� �ɸ��� �̸� �ټ��� Ǯ���صּ� �߰� ���� ����
    public void CreatePool(int count)
    {
        string[] prePools = { "Enemy", "Tree" }; // �� �� ���� ���� ���� �����Ǵٺ��� Ǯ�� �� ����� �κ��� �ִ�. �̸� Ǯ���صΰ� ���� ���� �߰� �� ���� ����
        for(int cnt = 0; cnt < count; cnt++)
        {
            foreach (string name in prePools)
            {
                GameObject select = Managers.Resource.Instantiate(name, _roots[name].transform);
                select.SetActive(false);
                _poolDict[name].Add(select);
            }
  
        }
    }

    public GameObject Get(string name)
    {
        GameObject select = null;
        foreach (GameObject prefab in _poolDict[name]) //�ش� �̸��� Ű�� �Ͽ� Ǯ�� ��Ͽ��� ã��
        {
            if(!prefab.activeSelf)  //��Ȱ��ȭ �� ��ü�� �ִٸ�
            {
                select = prefab;
                select.SetActive(true);
                break;
            }
        }
        if(select == null) //���� ��Ȱ��ȭ �� ��ü�� 1���� ���ٸ�
        {
            select = Managers.Resource.Instantiate(name, _roots[name].transform);
            _poolDict[name].Add(select);
        }
        return select;
    }

    public void Clear()
    {
        _prefabDict = null;
        _poolDict = null;
        _roots = null;
    }
}
