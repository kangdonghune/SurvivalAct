using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using static Define;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    private RectTransform _rect;
    private Item[] _items;

    public void Init()
    {
        _rect = GetComponent<RectTransform>();
        CreateItems();
        Hide();
    }

    public void CreateItems()
    {
        Transform parent = Managers.UI.Get<Transform>("ItemGroup"); //���� ���� ������ ���� â�� ������ �� �θ�
        if (parent.childCount != 0)
            return;
        foreach (var dataPair in Managers.Data.ItemDic)
        {
            Item newItem = Managers.Resource.Instantiate("Item").GetComponent<Item>();
            newItem.Init(dataPair.Value);
            newItem.GetComponent<RectTransform>().SetParent(parent);
            newItem.gameObject.SetActive(true);
        }
        Canvas.ForceUpdateCanvases();

        _items = GetComponentsInChildren<Item>(true);
        Managers.UI.UpdateDict(); // ui�� �߰��������� ��ųʸ� ����
    }

    public void Show()
    {
        Next();
        _rect.localScale = Vector3.one;
        Managers.Game.Stop();
        Managers.Audio.PlaySFX(AudioManager.SFX.LevelUp);
        Managers.Audio.EffectBGM(true);
    }

    public void Hide()
    {
        _rect.localScale = Vector3.zero;
        Managers.Game.Resume();
        Managers.Audio.PlaySFX(AudioManager.SFX.Select);
        Managers.Audio.EffectBGM(false);
    }

    public void Select(int idx)
    {
        _items[idx].OnClick();
    }

    public void Next()
    {
        // 1. ��� ������ ��Ȱ��ȭ
        foreach (Item item in _items)
            item.gameObject.SetActive(false);
        // 2. �� �߿��� �����ϰ� 3�� �����۸� Ȱ��ȭ
        int[] random = new int[3];
        while (true)
        {
            random[0] = Random.Range(0, _items.Length);
            random[1] = Random.Range(0, _items.Length);
            random[2] = Random.Range(0, _items.Length);

            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2])
                break;
        }
        // 3. ������ ���� �Һ� ���������� ��ü
        for (int idx = 0; idx < random.Length; idx++)
        {
            Item ranItem = _items[random[idx]];
            if (ranItem.Level == ranItem.Data.damages.Length)
            {
                _items[4].gameObject.SetActive(true);
            }
            else
                ranItem.gameObject.SetActive(true);
        }
    }
}
