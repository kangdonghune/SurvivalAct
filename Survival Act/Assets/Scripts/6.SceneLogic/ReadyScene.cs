using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static Define;

public class ReadyScene : BaseScene
{
    private bool[] _LoadEnds =  new bool[Enum.GetValues(typeof(LoadLabel)).Length]; //�� �� �� �ε� �Ϸ� ���� ����
    private bool _load = true;
    protected override void Init()
    {
        Application.targetFrameRate = 60; //�̰� ���� �����ָ� 30�����ӱ��� ��������.
        Type = SceneType.Ready; //�� �� ����
        //������ �ε� ����
        //�񵿱� �Լ��� int ++ �� ������ ������ ������ ���� ��. Ȥ�ö� �ε� ���� �ұ�Ģ�� ������ ����� Ȯ���� ��.
        Managers.Resource.LoadAllAsync<GameObject>(LoadLabel.Prefab.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Prefab] = key; });
        Managers.Resource.LoadAllAsync<RuntimeAnimatorController>(LoadLabel.Animation.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Animation] = key; });
        Managers.Resource.LoadAllAsync<TextAsset>(LoadLabel.Data.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Data] = key; });
        Managers.Resource.LoadAllAsync<AudioClip>(LoadLabel.Audio.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Audio] = key; });
        Managers.Resource.LoadAllAsync<GameObject>(LoadLabel.UI.ToString(), (key) => { _LoadEnds[(int)LoadLabel.UI] = key; });
        Managers.Resource.LoadAllAsync<Sprite>(LoadLabel.Sprite.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Sprite] = key; });
        StartCoroutine(CoCheckLoading());
    }
    public override void Clear() // ���� ���� ��� �� �Ѱ��Ŵ������� ����Ŵ��� ����
    {
        
    }

    IEnumerator CoCheckLoading()
    {
        while(true)
        {
            foreach (bool isload in _LoadEnds) // ��� �ε� ���� ��ȸ�ϸ� �ϳ��� �ε� �ȵǾ� �ִٸ� _load = false;
                _load = isload;
            if(_load == true)
            {
                Managers.Data.Init(); // json�� addressable�� �ܾ� �� �������� json�� �о�� �Ѵ�.
                Managers.UI.Get<Button>("StartButton").enabled = true; //������ ���� �ε��Ǹ� ��ư Ȱ��ȭ;
                Managers.Audio.Init();
                Managers.Game.Init();
                Managers.Achive.Init();
                Managers.Audio.PlayBGM(true);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.2f); //������ �����־ �ε��� ����Ǵ� ���� Ÿ������ 
        }

    }
}
