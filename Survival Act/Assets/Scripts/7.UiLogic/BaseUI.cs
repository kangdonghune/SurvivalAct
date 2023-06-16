using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    //�� ������ ��� ��Ʈ UI�� ���Ͽ� UI�� �Ѱ��ϰ� ���� �Ѵ� �� ����
    private Dictionary<string, GameObject> _ChildDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        //�� ���� �� ��Ʈ UI ������Ʈ�� �ڽĵ��� �����Ͽ� ����
        UpdateDict();
        Init();
    }

    public abstract void Init();

    public void UpdateDict()
    {
        _ChildDic.Clear(); // Clear�� ������ ���ܵδ� null ��� ���� ����.

        Transform[] childs = gameObject.GetComponentsInChildren<Transform>(true); //(true)�� ����� ��Ȱ��ȭ �� ui �鵵 �����ȴ�.

        for (int idx = 0; idx < childs.Length; idx++)
        {
            _ChildDic.Add(childs[idx].name, childs[idx].gameObject);
        }
    }

    public T Get<T>(string name = null) where T : Object
    {
        if (name == null)
            name = typeof(T).Name;
        if (_ChildDic.ContainsKey(name) == false) // �ش� Ű�� ���ٸ�
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
