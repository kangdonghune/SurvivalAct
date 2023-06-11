using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PoolManager : MonoBehaviour
{
    // 프리팹을 보관할 변수
    public Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();
    // 풀 담당을 하는 리스트
    private Dictionary<string, List<GameObject>> _poolDict = new Dictionary<string, List<GameObject>>();
    // 프리팹 별 루트
    private Dictionary<string, GameObject> _roots = new Dictionary<string, GameObject>();

    private void Awake()
    {
        //json으로 정의해둔 풀링 목록을 프리팹 딕셔너리로 전환
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

    //몬스터 같은 경우는 한 번에 왁 등장하는 패턴 등에서 소환 렉이 걸리니 미리 다수를 풀링해둬서 중간 렉을 방지
    public void CreatePool(int count)
    {
        string[] prePools = { "Enemy", "Tree" }; // 이 두 개는 수가 많이 생성되다보니 풀이 못 따라는 부분이 있다. 미리 풀링해두고 쓰면 게임 중간 렉 방지 가능
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
        foreach (GameObject prefab in _poolDict[name]) //해당 이름을 키로 하여 풀링 목록에서 찾기
        {
            if(!prefab.activeSelf)  //비활성화 된 개체가 있다면
            {
                select = prefab;
                select.SetActive(true);
                break;
            }
        }
        if(select == null) //현재 비활성화 된 개체가 1개도 없다면
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
