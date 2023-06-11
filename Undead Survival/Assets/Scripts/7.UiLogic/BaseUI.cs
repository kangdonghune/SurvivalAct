using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    //각 씬마다 담당 루트 UI가 산하에 UI를 총괄하고 끄고 켜는 걸 관리
    private Dictionary<string, GameObject> _ChildDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        //씬 시작 시 루트 UI 오브젝트가 자식들을 수집하여 정리
        UpdateDict();
        Init();
    }

    public abstract void Init();

    public void UpdateDict()
    {
        _ChildDic.Clear(); // Clear는 공간을 남겨두니 null 대신 쓰면 좋다.

        Transform[] childs = gameObject.GetComponentsInChildren<Transform>(true); //(true)로 해줘야 비활성화 된 ui 들도 수집된다.

        for (int idx = 0; idx < childs.Length; idx++)
        {
            _ChildDic.Add(childs[idx].name, childs[idx].gameObject);
        }
    }

    public T Get<T>(string name = null) where T : Object
    {
        if (name == null)
            name = typeof(T).Name;
        if (_ChildDic.ContainsKey(name) == false) // 해당 키가 없다면
        {
            return null;
        }

        return _ChildDic[name].GetComponent<T>();
    }

    private void OnDestroy()
    {
        _ChildDic = null;
    }
}
