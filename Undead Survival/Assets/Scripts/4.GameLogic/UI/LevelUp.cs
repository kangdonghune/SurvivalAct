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
        Transform parent = Managers.UI.Get<Transform>("ItemGroup"); //새로 만든 아이템 선택 창을 연결해 줄 부모
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
        Managers.UI.UpdateDict(); // ui를 추가해줬으니 딕셔너리 갱신
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
        // 1. 모든 아이템 비활성화
        foreach (Item item in _items)
            item.gameObject.SetActive(false);
        // 2. 그 중에서 랜덤하게 3개 아이템만 활성화
        int[] random = new int[3];
        while (true)
        {
            random[0] = Random.Range(0, _items.Length);
            random[1] = Random.Range(0, _items.Length);
            random[2] = Random.Range(0, _items.Length);

            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2])
                break;
        }
        // 3. 만렙이 장비는 소비 아이템으로 대체
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
